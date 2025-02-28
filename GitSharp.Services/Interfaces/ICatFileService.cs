using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface ICatFileService
{
    Task<FileType> GetObjectTypeAsync(string hash);

    Task<long> GetSizeAsync(string hash);
}