using System;
using System.IO;

namespace Pluto.Utilities
{
    internal static class Registry
    {
        internal static Lazy<Memory<byte>> NtdllBytes { get; }

        internal static Lazy<Memory<byte>> Win32UBytes { get; }

        static Registry()
        {
            NtdllBytes = new Lazy<Memory<byte>>(() => File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "ntdll.dll")));

            Win32UBytes = new Lazy<Memory<byte>>(() => File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "win32u.dll")));
        }
    }
}