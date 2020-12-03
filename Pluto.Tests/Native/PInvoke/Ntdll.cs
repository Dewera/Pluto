using System.Runtime.InteropServices;
using Pluto.Tests.Native.Enumerations;

namespace Pluto.Tests.Native.PInvoke
{
    internal static class Ntdll
    {
        [DllImport("ntdll.dll")]
        internal static extern int RtlNtStatusToDosError(NtStatus status);
    }
}