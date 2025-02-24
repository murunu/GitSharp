using System.IO.Compression;
using System.Text;
using GitSharp.Services.Interfaces;
using GitSharp.Services.Models;

namespace GitSharp.Services.Implementations;

public class FileSystemService(IDirectoryService directoryService) : IFileSystemService
{
    public async Task<string> GetContentFromHashAsync(string hash)
    {
        await using var fileStream = GetStream(hash);
        await using var zlibStream = new ZLibStream(fileStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        var content = await reader.ReadToEndAsync();
        return content;
    }
    
    public Stream GetStream(string hash)
    {
        var path = directoryService.GetPath(".gitsharp", "objects", hash[..2], hash[2..]);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Object not found", path);
        }

        return File.OpenRead(path);
    }
    
    public async Task WriteObjectAsync(string hash, FileTypes fileType, Stream stream)
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

    public async Task<string> ReadObjectAsync(string hash)
    {
        var content = await GetContentFromHashAsync(hash);

        var type = content.Split(" ");

        return string.Join(" ", type[1..]);
    }
}