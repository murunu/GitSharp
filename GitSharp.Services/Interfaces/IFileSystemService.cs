using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface IFileSystemService
{
    Task WriteObjectAsync(string hash, FileType fileType, Stream stream);

    Task<string> ReadObjectAsync(string hash);

    Task<string> GetContentFromHashAsync(string hash);

    Stream GetStream(string hash);
}