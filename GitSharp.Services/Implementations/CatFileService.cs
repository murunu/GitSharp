using GitSharp.Services.Interfaces;
using GitSharp.Services.Models;

namespace GitSharp.Services.Implementations;

public class CatFileService(IFileSystemService fileSystemService) : ICatFileService
{
    public async Task<FileType> GetObjectTypeAsync(string hash)
    {
        var content = await fileSystemService.GetContentFromHashAsync(hash);

        var type = content.Split(" ")[0];
        return Enum.Parse<FileType>(type, true);
    }

    public async Task<long> GetSizeAsync(string hash)
    {
        await using var stream = fileSystemService.GetStream(hash);

        return stream.Length;
    }
}