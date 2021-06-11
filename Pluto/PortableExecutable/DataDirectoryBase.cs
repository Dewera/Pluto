using System;
using System.Reflection.PortableExecutable;

namespace Pluto.PortableExecutable
{
    internal abstract class DataDirectoryBase
    {
        private protected int DirectoryOffset { get; }
        private protected Memory<byte> ImageBytes { get; }
        private protected bool IsValid { get; }

        private readonly PEHeaders _headers;

        private protected DataDirectoryBase(DirectoryEntry directory, PEHeaders headers, Memory<byte> imageBytes)
        {
            headers.TryGetDirectoryOffset(directory, out var directoryOffset);

            _headers = headers;

            DirectoryOffset = directoryOffset;
            ImageBytes = imageBytes;
            IsValid = directoryOffset != -1;
        }

        private protected int RvaToOffset(int rva)
        {
            var sectionHeader = _headers.SectionHeaders[_headers.GetContainingSectionIndex(rva)];

            return rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
        }
    }
}