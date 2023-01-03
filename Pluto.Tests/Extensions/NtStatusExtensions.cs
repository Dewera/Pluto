using Pluto.Tests.Native.Enums;

namespace Pluto.Tests.Extensions;

internal static class NtStatusExtensions
{
    internal static bool IsSuccess(this NtStatus status)
    {
        return (int) status >= 0;
    }
}