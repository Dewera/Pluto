using System.Reflection.PortableExecutable;

namespace Pluto.PortableExecutable;

internal abstract class DataDirectoryBase
{
    protected private int DirectoryOffset { get; }
    protected private Memory<byte> ImageBytes { get; }
    protected private bool IsValid { get; }

    private readonly PEHeaders _headers;

    protected private DataDirectoryBase(DirectoryEntry directory, PEHeaders headers, Memory<byte> imageBytes)
    {
        headers.TryGetDirectoryOffset(directory, out var directoryOffset);

        _headers = headers;

        DirectoryOffset = directoryOffset;
        ImageBytes = imageBytes;
        IsValid = directoryOffset != -1;
    }

    protected private int RvaToOffset(int rva)
    {
        var sectionHeader = _headers.SectionHeaders[_headers.GetContainingSectionIndex(rva)];

        return rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
    }
}