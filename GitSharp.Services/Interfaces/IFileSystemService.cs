using GitSharp.Services.Models;

namespace GitSharp.Services.Interfaces;

public interface IFileSystemService
{
    Task WriteObjectAsync(string hash, FileTypes fileType, Stream stream);

    Task<string> ReadObjectAsync(string hash);

    Task<string> GetContentFromHashAsync(string hash);

    Stream GetStream(string hash);
}