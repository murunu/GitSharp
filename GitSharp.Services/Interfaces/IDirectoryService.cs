namespace GitSharp.Services.Interfaces;

public interface IDirectoryService
{
    string GetPath(params string[] paths);
    
    bool IsInitialized { get; }
}