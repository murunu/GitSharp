using System.Diagnostics.CodeAnalysis;
using GitSharp.Services.Configuration;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GitSharp.Services.Implementations;

/// <summary>
/// Service to handle directory operations.
/// </summary>
[ExcludeFromCodeCoverage]
public class DirectoryService(IOptions<GitSharpConfiguration> gitSharpConfiguration) : IDirectoryService
{
    private readonly GitSharpConfiguration _gitSharpConfiguration = gitSharpConfiguration.Value;

    public string GetPath(params string[] paths)
        => Path.Combine([Directory.GetCurrentDirectory(), .. paths]);

    public bool IsInitialized =>
        Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName))
        && Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "objects"))
        && Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs"))
        && Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs/heads"))
        && Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs/remotes"))
        && Directory.Exists(GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs/tags"));
}