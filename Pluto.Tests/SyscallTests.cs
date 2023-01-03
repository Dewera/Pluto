using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pluto.Tests.Extensions;
using Pluto.Tests.Native.PInvoke;
using Xunit;

namespace Pluto.Tests;

public sealed class SyscallTests : IDisposable
{
    private readonly Process _process;
    private readonly nint _testAddress;
    private readonly int _testValue;

    public SyscallTests()
    {
        _process = Process.GetCurrentProcess();
        _testAddress = Marshal.AllocHGlobal(Environment.SystemPageSize);
        _testValue = 1024;
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(_testAddress);
    }

    [Fact]
    public void TestNtReadVirtualMemory()
    {
        Marshal.WriteInt32(_testAddress, _testValue);

        Span<byte> bytes = stackalloc byte[sizeof(int)];

        var syscall = new Syscall<Signatures.NtReadVirtualMemory>();
        var status = syscall.Method(_process.SafeHandle, _testAddress, out bytes[0], bytes.Length, out _);

        if (!status.IsSuccess())
        {
            throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
        }

        Assert.Equal(_testValue, MemoryMarshal.Read<int>(bytes));
    }

    [Fact]
    public void TestNtWriteVirtualMemory()
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        MemoryMarshal.Write(bytes, ref Unsafe.AsRef(_testValue));

        var syscall = new Syscall<Signatures.NtWriteVirtualMemory>();
        var status = syscall.Method(_process.SafeHandle, _testAddress, in bytes[0], bytes.Length, out _);

        if (!status.IsSuccess())
        {
            throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
        }

        Assert.Equal(_testValue, Marshal.ReadInt32(_testAddress));
    }
}