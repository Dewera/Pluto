using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using Pluto.Native.Structs;
using Pluto.PortableExecutable.Records;

namespace Pluto.PortableExecutable.DataDirectories
{
    internal sealed class ExportDirectory : DataDirectoryBase
    {
        internal ExportDirectory(PEHeaders headers, Memory<byte> imageBytes) : base(headers.PEHeader!.ExportTableDirectory, headers, imageBytes) { }

        internal ExportedFunction? GetExportedFunction(string functionName)
        {
            if (!IsValid)
            {
                return null;
            }

            // Read the export directory

            var exportDirectory = MemoryMarshal.Read<ImageExportDirectory>(ImageBytes.Span[DirectoryOffset..]);

            // Search the name address table for the corresponding name

            var low = 0;
            var high = exportDirectory.NumberOfNames - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                // Read the name at the current index

                var currentNameOffsetOffset = RvaToOffset(exportDirectory.AddressOfNames) + sizeof(int) * middle;
                var currentNameOffset = RvaToOffset(MemoryMarshal.Read<int>(ImageBytes.Span[currentNameOffsetOffset..]));
                var currentNameLength = ImageBytes.Span[currentNameOffset..].IndexOf(byte.MinValue);
                var currentName = Encoding.UTF8.GetString(ImageBytes.Span.Slice(currentNameOffset, currentNameLength));

                if (functionName.Equals(currentName, StringComparison.OrdinalIgnoreCase))
                {
                    // Read the function ordinal

                    var functionOrdinalOffset = RvaToOffset(exportDirectory.AddressOfNameOrdinals) + sizeof(short) * middle;
                    var functionOrdinal = MemoryMarshal.Read<short>(ImageBytes.Span[functionOrdinalOffset..]);

                    // Read the function address

                    var functionAddressOffset = RvaToOffset(exportDirectory.AddressOfFunctions) + sizeof(int) * functionOrdinal;
                    var functionAddress = MemoryMarshal.Read<int>(ImageBytes.Span[functionAddressOffset..]);

                    return new ExportedFunction(RvaToOffset(functionAddress));
                }

                // Adjust high/low according to binary search rules

                if (string.CompareOrdinal(functionName, currentName) < 0)
                {
                    high = middle - 1;
                }

                else
                {
                    low = middle + 1;
                }
            }

            return null;
        }
    }
}