using System;
using System.Runtime.InteropServices;
using Pluto.Native.Enumerations;

namespace Pluto.Native.PInvoke
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtect(IntPtr address, nint size, ProtectionType protectionType, out ProtectionType oldProtectionType);
    }
}