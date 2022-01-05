using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Pluto.Native;
using Pluto.Native.Enums;
using Pluto.Native.PInvoke;
using Pluto.PortableExecutable;
using Pluto.Shellcode;
using Pluto.Shellcode.Records;
using Pluto.Utilities;

namespace Pluto;

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

        if (!syscallImport.DllName.Equals("ntdll.dll", StringComparison.OrdinalIgnoreCase) && !syscallImport.DllName.Equals("win32u.dll", StringComparison.OrdinalIgnoreCase))
        {
            throw new NotSupportedException("The provided DLL does not export any syscalls");
        }

        Method = CreateSyscall(syscallImport.DllName, typeof(T).Name);
    }

    private static T CreateSyscall(string dllName, string entryPoint)
    {
        var dllBytes = dllName.Equals("ntdll.dll", StringComparison.OrdinalIgnoreCase) ? Registry.NtdllBytes : Registry.Win32UBytes;

        // Look for the function in the DLL

        var peImage = new PeImage(dllBytes);
        var function = peImage.ExportDirectory.GetExportedFunction(entryPoint);

        if (function is null)
        {
            throw new EntryPointNotFoundException($"Failed to find the function {entryPoint} in the module {dllName}");
        }

        // Read the syscall index

        var indexOffset = Environment.Is64BitProcess ? Constants.SyscallIndexOffset64 : Constants.SyscallIndexOffset32;
        var syscallIndex = MemoryMarshal.Read<int>(dllBytes.Span[(function.Offset + indexOffset)..]);

        // Assemble the shellcode used to perform the syscall

        var descriptor = new SyscallDescriptor(syscallIndex);
        var shellcodeBytes = Environment.Is64BitProcess ? Assembler.AssembleSyscall64(descriptor) : Assembler.AssembleSyscall32(descriptor);

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
}