using System.Runtime.InteropServices;
using Pluto.Shellcode.Records;

namespace Pluto.Shellcode;

internal static class Assembler
{
    internal static Span<byte> AssembleSyscall32(SyscallDescriptor descriptor)
    {
        var shellcode = new List<byte>();

        // mov eax, Index

        shellcode.Add(0xB8);
        shellcode.AddRange(BitConverter.GetBytes(descriptor.Index));

        // call DWORD PTR ds:[Wow64Transition]

        shellcode.AddRange(new byte[] { 0xFF, 0x15 });
        shellcode.AddRange(BitConverter.GetBytes(NativeLibrary.GetExport(NativeLibrary.Load("ntdll.dll"), "Wow64Transition").ToInt32()));

        // ret

        shellcode.Add(0xC3);

        return CollectionsMarshal.AsSpan(shellcode);
    }

    internal static Span<byte> AssembleSyscall64(SyscallDescriptor descriptor)
    {
        var shellcode = new List<byte>();

        // mov r10, rcx

        shellcode.AddRange(new byte[] { 0x4C, 0x8B, 0xD1 });

        // mov eax, Index

        shellcode.Add(0xB8);
        shellcode.AddRange(BitConverter.GetBytes(descriptor.Index));

        // syscall

        shellcode.AddRange(new byte[] { 0xF, 0x5 });

        // ret

        shellcode.Add(0xC3);

        return CollectionsMarshal.AsSpan(shellcode);
    }
}