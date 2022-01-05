namespace Pluto.Utilities;

internal static class Registry
{
    internal static Memory<byte> NtdllBytes { get; }
    internal static Memory<byte> Win32UBytes { get; }

    static Registry()
    {
        var systemDirectoryPath = Environment.SystemDirectory;

        if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
        {
            systemDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        }

        NtdllBytes = File.ReadAllBytes(Path.Combine(systemDirectoryPath, "ntdll.dll"));
        Win32UBytes = File.ReadAllBytes(Path.Combine(systemDirectoryPath, "win32u.dll"));
    }
}