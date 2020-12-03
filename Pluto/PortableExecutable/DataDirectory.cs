using System;
using System.Reflection.PortableExecutable;

namespace Pluto.PortableExecutable
{
    internal abstract class DataDirectory
    {
        private protected int DirectoryOffset { get; }

        private protected Memory<byte> ImageBytes { get; }

        private protected bool IsValid { get; }

        private readonly PEHeaders _headers;

        private protected DataDirectory(PEHeaders headers, Memory<byte> imageBytes, DirectoryEntry directory)
        {
            _headers = headers;

            IsValid = headers.TryGetDirectoryOffset(directory, out var directoryOffset);

            DirectoryOffset = directoryOffset;

            ImageBytes = imageBytes;
        }

        private protected int RvaToOffset(int rva)
        {
            var sectionHeader = _headers.SectionHeaders[_headers.GetContainingSectionIndex(rva)];

            return rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
        }
    }
}