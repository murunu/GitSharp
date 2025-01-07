namespace GitSharp.Helpers;

public static class IgnoreList
{
    public static async Task<List<string>> GetIgnoreList()
        =>
        [
            ".gitsharp/",
            ..(await File.ReadAllLinesAsync(".gitsharpignore"))
            .Where(x => !x.StartsWith('#') && !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
        ];
}