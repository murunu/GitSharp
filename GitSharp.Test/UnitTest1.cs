using GitSharp.Extensions;

namespace GitSharp.Test;

public class UnitTest1
{
    private const string Path = @"C:\Users\moren\Documents\Github\GitSharp\GitSharp";
    
    [Fact]
    public async Task Test1()
    {
        var ignoreList = await GetIgnoreList();
        var res = Path.GetDirectories(ignoreList);
        var res2 = Path.GetFiles(ignoreList);
    }
    
    private static async Task<List<string>> GetIgnoreList()
        =>
        [
            ".gitsharp/",
            ..(await File.ReadAllLinesAsync(System.IO.Path.Combine(Path, ".gitsharpignore")))
            .Where(x => !x.StartsWith('#') && !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
        ];
}