namespace GitSharp.Services.Configuration;

public class GitSharpConfiguration
{
    public bool UseSha2 { get; init; }

    public string GitSharpDirectoryName { get; init; } = ".gitsharp";
}