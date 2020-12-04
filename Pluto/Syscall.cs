using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Pluto.Assembly;
using Pluto.Native;
using Pluto.Native.Enumerations;
using Pluto.Native.PInvoke;
using Pluto.PortableExecutable;

namespace Pluto
{
    /// <summary>
    /// Provides the functionality to syscall a function in a DLL
    /// </summary>
    public sealed class Syscall<T> where T : Delegate
    {
        /// <summary>
        /// A delegate wrapping the syscall
        /// </summary>
        public T Method { get; }

        /// <summary>
        /// Initialises an instance of the <see cref="Syscall{T}"/> class with the syscall delegate
        /// </summary>
        public Syscall()
        {
            var syscallImport = typeof(T).GetCustomAttribute<SyscallImportAttribute>();

            if (syscallImport is null)
            {
                throw new ArgumentException("The provided delegate was not attributed with the SyscallImport attribute");
            }

            if (!syscallImport.DllName.Equals("ntdll.dll", StringComparison.OrdinalIgnoreCase)
                && !syscallImport.DllName.Equals("win32u.dll", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("The provided DLL does not export any syscalls");
            }

            Method = CreateSyscall(syscallImport);
        }

        private static T CreateSyscall(SyscallImportAttribute syscallImport)
        {
            // Create the shellcode used to perform the syscall

            var syscallIndex = GetSyscallIndex(syscallImport.DllName, syscallImport.FunctionName);

            var shellcodeBytes = Environment.Is64BitProcess
                ? Assembler.AssembleSyscall64(syscallIndex)
                : Assembler.AssembleSyscall32(syscallIndex);

            // Write the shellcode into the pinned object heap

            var pinnedArray = GC.AllocateArray<byte>(shellcodeBytes.Length, true);

            shellcodeBytes.CopyTo(pinnedArray);

            // Wrap the shellcode in a managed delegate

            var shellcodeAddress = Marshal.UnsafeAddrOfPinnedArrayElement(pinnedArray, 0);

            if (!Kernel32.VirtualProtect(shellcodeAddress, shellcodeBytes.Length, ProtectionType.ExecuteReadWrite, out _))
            {
                throw new Win32Exception();
            }

            return Marshal.GetDelegateForFunctionPointer<T>(shellcodeAddress);
        }

        private static int GetSyscallIndex(string dllName, string functionName)
        {
            var dllBytes = File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, dllName));

            // Retrieve the function

            var peImage = new PeImage(dllBytes);

            var function = peImage.ExportDirectory.GetExportedFunction(functionName);

            if (function is null)
            {
                throw new EntryPointNotFoundException($"Failed to find the function {functionName} in the DLL {dllName}");
            }

            // Read the syscall index

            var functionBytes = dllBytes.AsSpan()[function.Offset..];

            return MemoryMarshal.Read<int>(
                Environment.Is64BitProcess
                ? functionBytes[Constants.SyscallIndexOffset64..]
                : functionBytes[Constants.SyscallIndexOffset32..]);
        }
    }
}