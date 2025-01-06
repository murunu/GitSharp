using System.IO.Compression;
using Cocona;
using GitSharp.Models;

namespace GitSharp.Commands;

public class CommitCommands
{
    [Command(nameof(Commit))]
    public async Task Commit([Argument] string message)
    {
        Console.WriteLine($"Committing with message: {message}");
    }

    public async Task CreateTree()
    {
        if(!Directory.Exists(".gitsharp"))
        {
            Directory.CreateDirectory(".gitsharp");
            Directory.CreateDirectory(".gitsharp/objects");
            Directory.CreateDirectory(".gitsharp/refs");
        }
        
        var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory());

        var tree = new Tree();
        foreach (var f in files)
        {
            var blob = new Blob(tree.Hash);
            await blob.CreateHash(File.OpenRead(f));
            await blob.Save();

            Console.WriteLine($"Created blob for file {f} with hash: {blob.HashString}");
        }
    }

    public async Task Status()
    {
        
    }

    public async Task GetByHash([Argument] string hash)
    {
        var path = $".gitsharp/objects/{hash[..2]}/{hash[2..]}";
        if (!File.Exists(path))
        {
            Console.WriteLine("Object not found.");
            return;
        }

        await using var sourceStream = File.OpenRead(path);
        
        await using var zlibStream = new ZLibStream(sourceStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        var result = await reader.ReadToEndAsync();
        Console.WriteLine(result);
    }
}