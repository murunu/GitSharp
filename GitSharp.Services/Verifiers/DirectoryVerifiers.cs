namespace GitSharp.Services.Verifiers;

public class DirectoryVerifiers
{
    public static bool FolderInitialized(string directory)
        => Directory.Exists(Path.Combine(directory, ".gitsharp"))
           && Directory.Exists(Path.Combine(directory, ".gitsharp/objects"))
           && Directory.Exists(Path.Combine(directory, ".gitsharp/refs"))
           && Directory.Exists(Path.Combine(directory, ".gitsharp/refs/heads"))
           && Directory.Exists(Path.Combine(directory, ".gitsharp/refs/remotes"))
           && Directory.Exists(Path.Combine(directory, ".gitsharp/refs/tags"));
}