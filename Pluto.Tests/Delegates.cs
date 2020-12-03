using System;
using Microsoft.Win32.SafeHandles;
using Pluto.Tests.Native.Enumerations;
using Pluto.Tests.Native.Structures;

namespace Pluto.Tests
{
    internal static class Delegates
    {
        [SyscallImport("ntdll.dll", "NtOpenProcess")]
        internal delegate NtStatus OpenProcess32(out SafeProcessHandle processHandle, AccessMask accessMask, in ObjectAttributes32 objectAttributes, in ClientId32 clientId);

        [SyscallImport("ntdll.dll", "NtOpenProcess")]
        internal delegate NtStatus OpenProcess64(out SafeProcessHandle processHandle, AccessMask accessMask, in ObjectAttributes64 objectAttributes, in ClientId64 clientId);

        [SyscallImport("ntdll.dll", "NtReadVirtualMemory")]
        internal delegate NtStatus ReadProcessMemory(SafeProcessHandle processHandle, IntPtr address, out byte bytes, int size, out int bytesRead);

        [SyscallImport("ntdll.dll", "NtWriteVirtualMemory")]
        internal delegate NtStatus WriteProcessMemory(SafeProcessHandle processHandle, IntPtr address, in byte bytes, int size, out int bytesWritten);
    }
}