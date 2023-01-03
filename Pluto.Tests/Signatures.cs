using Microsoft.Win32.SafeHandles;
using Pluto.Tests.Native.Enums;

namespace Pluto.Tests;

internal static class Signatures
{
    [SyscallImport("ntdll.dll")]
    internal delegate NtStatus NtReadVirtualMemory(SafeProcessHandle processHandle, nint address, out byte bytes, int size, out int bytesRead);

    [SyscallImport("ntdll.dll")]
    internal delegate NtStatus NtWriteVirtualMemory(SafeProcessHandle processHandle, nint address, in byte bytes, int size, out int bytesWritten);
}