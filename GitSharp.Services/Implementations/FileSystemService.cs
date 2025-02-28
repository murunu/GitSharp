using System.IO.Compression;
using System.Text;
using GitSharp.Models;
using GitSharp.Services.Configuration;
using GitSharp.Services.Exceptions;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GitSharp.Services.Implementations;

public class FileSystemService(IDirectoryService directoryService, IOptions<GitSharpConfiguration> gitSharpConfiguration) : IFileSystemService
{
    private readonly GitSharpConfiguration _configuration = gitSharpConfiguration.Value;

    public async Task<string> GetContentFromHashAsync(string hash)
    {
        if (!directoryService.IsInitialized)
        {
            throw new DirectoryNotInitializedException();
        }

        await using var fileStream = GetStream(hash);
        await using var zlibStream = new ZLibStream(fileStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        var content = await reader.ReadToEndAsync();
        return content;
    }

    public Stream GetStream(string hash)
    {
        if (!directoryService.IsInitialized)
        {
            throw new DirectoryNotInitializedException();
        }

        var path = directoryService.GetPath(_configuration.GitSharpDirectoryName, "objects", hash[..2], hash[2..]);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Object not found", path);
        }

        return File.OpenRead(path);
    }

    public async Task WriteObjectAsync(string hash, FileType fileType, Stream stream)
    {
        if (!directoryService.IsInitialized)
        {
            throw new DirectoryNotInitializedException();
        }

        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var content = await streamReader.ReadToEndAsync();

        content = $"{fileType} {content}";

        var folder = directoryService.GetPath(_configuration.GitSharpDirectoryName, "objects", hash[..2]);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        await using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await using var fileStream = File.OpenWrite(Path.Combine(folder, hash[2..]));
        await using var zlibStream = new ZLibStream(fileStream, CompressionMode.Compress);

        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(zlibStream);
    }

    public async Task<string> ReadObjectAsync(string hash)
    {
        if (!directoryService.IsInitialized)
        {
            throw new DirectoryNotInitializedException();
        }

        var content = await GetContentFromHashAsync(hash);

        var type = content.Split(" ");

        return string.Join(" ", type[1..]);
    }
}