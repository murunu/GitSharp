namespace GitSharp.Commands;

public class InitCommands
{
    public void Init()
    {
        Console.WriteLine("Initializing GitSharp repository...");
        
        CreateDirectories();
        
        Console.WriteLine("GitSharp repository initialized.");
    }
    
    private static void CreateDirectories()
    {
        if (Directory.Exists(".gitsharp")) return;
        
        Directory.CreateDirectory(".gitsharp");
        Directory.CreateDirectory(".gitsharp/objects");
        Directory.CreateDirectory(".gitsharp/refs");
        Directory.CreateDirectory(".gitsharp/refs/heads");
        Directory.CreateDirectory(".gitsharp/refs/remotes");
        Directory.CreateDirectory(".gitsharp/refs/tags");
    }
}