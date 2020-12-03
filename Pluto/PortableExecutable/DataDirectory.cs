using System;
using System.Reflection.PortableExecutable;

namespace Pluto.PortableExecutable
{
    internal abstract class DataDirectory
    {
        private protected int DirectoryOffset { get; }

        private protected PEHeaders Headers { get; }

        private protected Memory<byte> ImageBytes { get; }

        private protected bool IsValid { get; }

        private protected DataDirectory(PEHeaders headers, Memory<byte> imageBytes, DirectoryEntry directory)
        {
            IsValid = headers.TryGetDirectoryOffset(directory, out var directoryOffset);

            DirectoryOffset = directoryOffset;

            Headers = headers;

            ImageBytes = imageBytes;
        }

        private protected int RvaToOffset(int rva)
        {
            var sectionHeader = Headers.SectionHeaders[Headers.GetContainingSectionIndex(rva)];

            return rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
        }
    }
}