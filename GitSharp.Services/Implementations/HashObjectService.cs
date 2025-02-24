using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using GitSharp.Services.Interfaces;
using GitSharp.Services.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Services.Implementations;

internal class HashObjectService(
    IDirectoryService directoryService,
    ILogger<HashObjectService> logger) : IHashObjectService
{
    public async Task<string> HashObject(string path, FileTypes type = FileTypes.Blob, bool write = false)
    {
        if (!File.Exists(directoryService.GetPath(path)))
        {
            throw new FileNotFoundException("File not found", directoryService.GetPath(path));
        }
        
        await using var fileStream = File.OpenRead(directoryService.GetPath(path));
        var hash = await HashDataAsync(fileStream);

        if (write)
        {
            await WriteObjectAsync(hash, type, fileStream);
            
            logger.LogDebug("Object written to disk with hash: {Hash}", hash);
        }
        
        return hash;
    }
    
    private static async Task<string> HashDataAsync(Stream stream)
    {
        using var sha1 = SHA1.Create();
        return Convert.ToHexString(await sha1.ComputeHashAsync(stream)).ToLower();
    }
    
    private async Task WriteObjectAsync(string hash, FileTypes fileType, Stream stream)
    {
        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var content = await streamReader.ReadToEndAsync();
        
        content = $"{fileType} {content}";
        
        var folder = directoryService.GetPath(".gitsharp", "objects", hash[..2]);
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
    
    public async Task<FileTypes> GetObjectTypeAsync(string hash)
    {
        var content = await GetContentFromHashAsync(hash);

        var type = content.Split(" ")[0];
        return Enum.Parse<FileTypes>(type, true);
    }
    
    public async Task<long> GetSizeAsync(string hash)
    {
        await using var stream = GetFileStreamAsync(hash);

        return stream.Length;
    }
    
    public async Task<string> ReadObjectAsync(string hash)
    {
        var content = await GetContentFromHashAsync(hash);

        var type = content.Split(" ");

        return string.Join(" ", type[1..]);
    }

    private async Task<string> GetContentFromHashAsync(string hash)
    {
        await using var fileStream = GetFileStreamAsync(hash);
        await using var zlibStream = new ZLibStream(fileStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        var content = await reader.ReadToEndAsync();
        return content;
    }

    private FileStream GetFileStreamAsync(string hash)
    {
        var path = directoryService.GetPath(".gitsharp", "objects", hash[..2], hash[2..]);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Object not found", path);
        }

        return File.OpenRead(path);
    }
}