using System.Runtime.InteropServices;
using Pluto.Native.Enums;

namespace Pluto.Native.PInvoke;

internal static partial class Kernel32
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool VirtualProtect(nint address, nint size, ProtectionType protectionType, out ProtectionType oldProtectionType);
}