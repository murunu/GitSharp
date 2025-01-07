namespace GitSharp.Extensions;

public static class DirectoryExtensions
{
    public static IEnumerable<string> GetFiles(this string path, List<string> ignoreList) 
        => Directory.EnumerateFiles(path)
        .Select(file => Path.GetRelativePath(Directory.GetCurrentDirectory(), file))
        .Where(file => !ignoreList.Any(file.StartsWith));
    
    public static IEnumerable<string> GetDirectories(this string path, List<string> ignoreList) 
        => Directory.EnumerateDirectories(path)
            .Select(directory => Path.GetRelativePath(Directory.GetCurrentDirectory(), directory).Replace("\\", "/") + '/')
            .Where(directory => !ignoreList.Any(directory.StartsWith));
}