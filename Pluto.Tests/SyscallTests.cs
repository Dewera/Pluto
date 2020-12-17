using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pluto.Tests.Native.Enumerations;
using Pluto.Tests.Native.PInvoke;
using Xunit;

namespace Pluto.Tests
{
    public sealed class SyscallTests : IDisposable
    {
        private readonly Process _process;

        private readonly IntPtr _testAddress;

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

            var syscall = new Syscall<Signatures.NtReadVirtualMemory>();

            Span<byte> bytes = stackalloc byte[sizeof(int)];

            var status = syscall.Method(_process.SafeHandle, _testAddress, out bytes[0], bytes.Length, out _);

            if (status != NtStatus.Success)
            {
                throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
            }

            Assert.Equal(_testValue, MemoryMarshal.Read<int>(bytes));
        }

        [Fact]
        public void TestNtWriteVirtualMemory()
        {
            var syscall = new Syscall<Signatures.NtWriteVirtualMemory>();

            Span<byte> bytes = stackalloc byte[sizeof(int)];

            MemoryMarshal.Write(bytes, ref Unsafe.AsRef(_testValue));

            var status = syscall.Method(_process.SafeHandle, _testAddress, in bytes[0], bytes.Length, out _);

            if (status != NtStatus.Success)
            {
                throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
            }

            Assert.Equal(_testValue, Marshal.ReadInt32(_testAddress));
        }
    }
}