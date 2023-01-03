using System.Reflection.PortableExecutable;
using Pluto.PortableExecutable.DataDirectories;

namespace Pluto.PortableExecutable;

internal sealed class PeImage
{
    internal ExportDirectory ExportDirectory { get; }

    internal PeImage(Memory<byte> imageBytes)
    {
        using var peReader = new PEReader(new MemoryStream(imageBytes.ToArray()));

        if (peReader.PEHeaders.PEHeader is null || !peReader.PEHeaders.IsDll)
        {
            throw new BadImageFormatException("The provided file was not a valid DLL");
        }

        ExportDirectory = new ExportDirectory(peReader.PEHeaders, imageBytes);
    }
}