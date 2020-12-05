using System.Runtime.InteropServices;

namespace Pluto.Tests.Native.Structures
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    internal readonly struct ClientId32
    {
        [FieldOffset(0x0)]
        private readonly int UniqueProcess;

        internal ClientId32(int uniqueProcess)
        {
            UniqueProcess = uniqueProcess;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    internal readonly struct ClientId64
    {
        [FieldOffset(0x0)]
        private readonly long UniqueProcess;

        internal ClientId64(long uniqueProcess)
        {
            UniqueProcess = uniqueProcess;
        }
    }
}