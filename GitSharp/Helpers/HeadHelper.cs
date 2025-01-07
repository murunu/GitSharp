using System.Text;

namespace GitSharp.Helpers;

public static class HeadHelper
{
    public static void SetCommit(string commitHash)
    {
        File.WriteAllText(GetCurrentHead(), commitHash);
    }
    
    private static string GetCurrentHead()
    {
        const string headPath = ".gitsharp/HEAD";
        if (!File.Exists(headPath))
        {
            File.WriteAllText(headPath, "refs/heads/main");
        }
        
        var currentHead = File.ReadAllText(headPath);
        return $".gitsharp/{currentHead}";
    }

    public static string GetCurrentCommitHash()
    {
        return File.Exists(GetCurrentHead()) ? File.ReadAllText(GetCurrentHead()) : "";
    }
}