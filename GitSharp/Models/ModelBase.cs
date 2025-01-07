using System.IO.Compression;
using System.Security.Cryptography;
using GitSharp.Exceptions;

namespace GitSharp.Models;

/// <summary>
/// Name To be changed to something more meaningful.
/// </summary>
public abstract class ModelBase
{
    public byte[] Hash { get; protected set; } = new byte[20];

    public string HashString => Convert.ToHexString(Hash);

    private static void VerifyHash(byte[] hash)
    {
        if (!hash.Length.Equals(20))
        {
            throw new InvalidHashException();
        }
    }

    public virtual async Task CreateHash(Stream source, bool save = false)
    {
        await using var stream = new MemoryStream();
        await source.CopyToAsync(stream);

        await CreateHash(stream, save);
    }
    
    public virtual async Task CreateHash(MemoryStream stream, bool save = false)
    {
        if (!SHA1.TryHashData(stream.ToArray(), Hash, out _))
        {
            throw new Exception("Failed to create hash.");
        }

        // Reset the stream position to the beginning.
        stream.Position = 0;

        VerifyHash(Hash);

        if(!save) return;
        await Save(stream);
    }

    private async Task Save(MemoryStream stream)
    {
        if (!Directory.Exists(".gitsharp/objects"))
        {
            Directory.CreateDirectory(".gitsharp/objects");
        }
        
        if (!Directory.Exists(".gitsharp/objects/" + HashString[..2]))
        {
            Directory.CreateDirectory(".gitsharp/objects/" + HashString[..2]);
        }
        await using var targetStream = File.OpenWrite($".gitsharp/objects/{HashString[..2]}/{HashString[2..]}");
        
        await using var zlibStream = new ZLibStream(targetStream, CompressionMode.Compress);
        
        await stream.CopyToAsync(zlibStream);
        
        stream.Close();
        zlibStream.Close();
        targetStream.Close();
    }
}