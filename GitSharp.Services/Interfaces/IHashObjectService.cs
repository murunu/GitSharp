using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface IHashObjectService
{
    Task<string> HashObject(string path, FileType type = FileType.Blob, bool write = false);
}