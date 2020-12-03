using System.Runtime.InteropServices;

namespace Pluto.Tests.Native.Structures
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    internal readonly struct ObjectAttributes32 { }

    [StructLayout(LayoutKind.Explicit, Size = 48)]
    internal readonly struct ObjectAttributes64 { }
}