namespace GitSharp.Verifiers;

public static class DirectoryVerifiers
{
    public static bool FolderInitialized()
    {
        return Directory.Exists(".gitsharp")
            && Directory.Exists(".gitsharp/objects")
            && Directory.Exists(".gitsharp/refs")
            && Directory.Exists(".gitsharp/refs/heads")
            && Directory.Exists(".gitsharp/refs/remotes")
            && Directory.Exists(".gitsharp/refs/tags");
    }
}