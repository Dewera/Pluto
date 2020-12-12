using System;
using System.IO;

namespace Pluto.Utilities
{
    internal static class Registry
    {
        internal static Memory<byte> NtdllBytes { get; }

        internal static Memory<byte> Win32UBytes { get; }

        static Registry()
        {
            NtdllBytes = File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "ntdll.dll"));

            Win32UBytes = File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "win32u.dll"));
        }
    }
}