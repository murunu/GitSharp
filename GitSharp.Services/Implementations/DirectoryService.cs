using System.Diagnostics.CodeAnalysis;
using GitSharp.Services.Interfaces;

namespace GitSharp.Services.Implementations;

/// <summary>
/// Service to handle directory operations.
/// </summary>
[ExcludeFromCodeCoverage]
public class DirectoryService : IDirectoryService
{
    public string GetPath(params string[] paths) 
        => Path.Combine([Directory.GetCurrentDirectory(), ..paths]);
}