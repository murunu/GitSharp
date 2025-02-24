using GitSharp.Services.Interfaces;

namespace GitSharp.Services.Implementations;

public class DirectoryService : IDirectoryService
{
    private string GetCurrentDirectory()
    {
        return Directory.GetCurrentDirectory();
    }

    public string GetPath(params string[] paths)
    {
        return Path.Combine([GetCurrentDirectory(), ..paths]);
    }
}