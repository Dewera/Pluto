using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pluto.Tests.Native.Enumerations;
using Pluto.Tests.Native.PInvoke;
using Pluto.Tests.Native.Structures;
using Xunit;

namespace Pluto.Tests
{
    public sealed class SyscallTests : IDisposable
    {
        private readonly Process _process;

        private readonly IntPtr _testAddress;

        public SyscallTests()
        {
            _process = Process.GetCurrentProcess();

            _testAddress = Marshal.AllocHGlobal(Environment.SystemPageSize);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_testAddress);
        }

        [Fact]
        public void TestNtOpenProcess()
        {
            if (Environment.Is64BitProcess)
            {
                var syscall = new Syscall<Delegates.OpenProcess64>();

                var status = syscall.Method(out var processHandle, AccessMask.ProcessAllAccess, new ObjectAttributes64(), new ClientId64(_process.Id));

                if (status != NtStatus.Success)
                {
                    throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
                }

                Assert.False(processHandle.IsInvalid);
            }
            else
            {
                var syscall = new Syscall<Delegates.OpenProcess32>();

                var status = syscall.Method(out var processHandle, AccessMask.ProcessAllAccess, new ObjectAttributes32(), new ClientId32(_process.Id));

                if (status != NtStatus.Success)
                {
                    throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
                }

                Assert.False(processHandle.IsInvalid);
            }
        }

        [Fact]
        public void TestNtReadVirtualMemory()
        {
            const int testNumber = 1024;

            Marshal.WriteInt32(_testAddress, testNumber);

            var syscall = new Syscall<Delegates.ReadProcessMemory>();

            Span<byte> bytes = stackalloc byte[sizeof(int)];

            var status = syscall.Method(_process.SafeHandle, _testAddress, out bytes[0], bytes.Length, out _);

            if (status != NtStatus.Success)
            {
                throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
            }

            Assert.Equal(testNumber, MemoryMarshal.Read<int>(bytes));
        }

        [Fact]
        public void TestNtWriteVirtualMemory()
        {
            const int testNumber = 1024;

            var syscall = new Syscall<Delegates.WriteProcessMemory>();

            Span<byte> bytes = stackalloc byte[sizeof(int)];

            MemoryMarshal.Write(bytes, ref Unsafe.AsRef(testNumber));

            var status = syscall.Method(_process.SafeHandle, _testAddress, in bytes[0], bytes.Length, out _);

            if (status != NtStatus.Success)
            {
                throw new Win32Exception(Ntdll.RtlNtStatusToDosError(status));
            }

            Assert.Equal(testNumber, Marshal.ReadInt32(_testAddress));
        }
    }
}