namespace GitSharp.Services.Verifiers;

public class DirectoryVerifiers
{
    public static bool FolderInitialized(string directory, string folderName)
        => Directory.Exists(Path.Combine(directory, folderName))
           && Directory.Exists(Path.Combine(directory, folderName, "objects"))
           && Directory.Exists(Path.Combine(directory, folderName, "refs"))
           && Directory.Exists(Path.Combine(directory, folderName, "refs/heads"))
           && Directory.Exists(Path.Combine(directory, folderName, "refs/remotes"))
           && Directory.Exists(Path.Combine(directory, folderName, "refs/tags"));
}