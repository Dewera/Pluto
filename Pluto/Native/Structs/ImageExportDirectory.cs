using System.Runtime.InteropServices;

namespace Pluto.Native.Structs;

[StructLayout(LayoutKind.Explicit, Size = 40)]
internal readonly record struct ImageExportDirectory([field: FieldOffset(0x18)] int NumberOfNames, [field: FieldOffset(0x1C)] int AddressOfFunctions, [field: FieldOffset(0x20)] int AddressOfNames, [field: FieldOffset(0x24)] int AddressOfNameOrdinals);