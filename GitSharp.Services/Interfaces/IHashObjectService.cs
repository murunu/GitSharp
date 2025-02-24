using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface IHashObjectService
{
    Task<string> HashObject(string path, FileTypes type = FileTypes.Blob, bool write = false);
}