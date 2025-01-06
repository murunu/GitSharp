using System.IO.Compression;
using System.Security.Cryptography;
using GitSharp.Exceptions;

namespace GitSharp.Models;

/// <summary>
/// Name To be changed to something more meaningful.
/// </summary>
public class ModelBase
{
    public byte[] Hash { get; protected set; } = new byte[20];

    public string HashString => Convert.ToHexString(Hash);
    
    public byte[]? ParentHash { get; protected set; }

    private readonly MemoryStream _stream = new();
    
    protected static void VerifyHash(byte[] hash)
    {
        if (!hash.Length.Equals(20))
        {
            throw new InvalidHashException();
        }
    }

    public async Task CreateHash(Stream source)
    {
        await source.CopyToAsync(_stream);
        if (!SHA1.TryHashData(_stream.ToArray(), Hash, out var written))
        {
            Console.WriteLine(written);
            throw new Exception("Failed to create hash.");
        }

        _stream.Position = 0;

        VerifyHash(Hash);
    }

    public async Task SetParentHash(Stream source)
    {
        ParentHash = await SHA1.HashDataAsync(source);

        VerifyHash(ParentHash);
    }

    public async Task Save()
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
        
        await _stream.CopyToAsync(zlibStream);
        
        _stream.Close();
        await _stream.DisposeAsync();
        
        zlibStream.Close();
        targetStream.Close();
    }
}