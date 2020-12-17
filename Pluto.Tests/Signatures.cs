using System;
using Microsoft.Win32.SafeHandles;
using Pluto.Tests.Native.Enumerations;

namespace Pluto.Tests
{
    internal static class Signatures
    {
        [SyscallImport("ntdll.dll", "NtReadVirtualMemory")]
        internal delegate NtStatus NtReadVirtualMemory(SafeProcessHandle processHandle, IntPtr address, out byte bytes, int size, out int bytesRead);

        [SyscallImport("ntdll.dll", "NtWriteVirtualMemory")]
        internal delegate NtStatus NtWriteVirtualMemory(SafeProcessHandle processHandle, IntPtr address, in byte bytes, int size, out int bytesWritten);
    }
}