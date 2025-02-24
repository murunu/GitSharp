using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface IHashObjectService
{
    Task<string> HashObject(string path, FileTypes type = FileTypes.Blob, bool write = false);

    Task<FileTypes> GetObjectTypeAsync(string hash);
    
    Task<long> GetSizeAsync(string hash);
    
    Task<string> ReadObjectAsync(string hash);
}