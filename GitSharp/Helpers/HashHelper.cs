using System.IO.Compression;

namespace GitSharp.Helpers;

public static class HashHelper
{
    public static async Task<string?> GetByHash(string hash)
    {
        var path = $".gitsharp/objects/{hash[..2]}/{hash[2..]}";
        if (!File.Exists(path))
        {
            return null;
        }

        await using var sourceStream = File.OpenRead(path);
        
        await using var zlibStream = new ZLibStream(sourceStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        return await reader.ReadToEndAsync();
    }
}