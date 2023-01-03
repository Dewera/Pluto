using System.Runtime.InteropServices;
using Pluto.Tests.Native.Enums;

namespace Pluto.Tests.Native.PInvoke;

internal static partial class Ntdll
{
    [LibraryImport("ntdll.dll")]
    internal static partial int RtlNtStatusToDosError(NtStatus status);
}