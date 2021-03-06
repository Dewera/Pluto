﻿using System.Runtime.InteropServices;

namespace Pluto.Native.Structs
{
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    internal readonly struct ImageExportDirectory
    {
        [FieldOffset(0x18)]
        internal readonly int NumberOfNames;
        [FieldOffset(0x1C)]
        internal readonly int AddressOfFunctions;
        [FieldOffset(0x20)]
        internal readonly int AddressOfNames;
        [FieldOffset(0x24)]
        internal readonly int AddressOfNameOrdinals;
    }
}