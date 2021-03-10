using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Pluto.Assembly
{
    internal static class Assembler
    {
        internal static Span<byte> AssembleSyscall32(int syscallIndex)
        {
            var instructions = new List<byte>();

            // mov eax, syscallIndex

            instructions.Add(0xB8);

            instructions.AddRange(BitConverter.GetBytes(syscallIndex));

            if (Environment.Is64BitOperatingSystem)
            {
                // call DWORD PTR ds:[Wow64Transition]

                instructions.AddRange(new byte[] {0xFF, 0x15});

                instructions.AddRange(BitConverter.GetBytes(NativeLibrary.GetExport(NativeLibrary.Load("ntdll.dll"), "Wow64Transition").ToInt32()));
            }

            else
            {
                // mov edx, esp

                instructions.AddRange(new byte[] {0x89, 0xE2});

                // sysenter

                instructions.AddRange(new byte[] {0xF, 0x34});
            }

            // ret

            instructions.Add(0xC3);

            return CollectionsMarshal.AsSpan(instructions);
        }

        internal static Span<byte> AssembleSyscall64(int syscallIndex)
        {
            var instructions = new List<byte>();

            // mov r10, rcx

            instructions.AddRange(new byte[] {0x4C, 0x8B, 0xD1});

            // mov eax, syscallIndex

            instructions.Add(0xB8);

            instructions.AddRange(BitConverter.GetBytes(syscallIndex));

            // syscall

            instructions.AddRange(new byte[] {0xF, 0x5});

            // ret

            instructions.Add(0xC3);

            return CollectionsMarshal.AsSpan(instructions);
        }
    }
}