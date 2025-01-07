using System.IO.Compression;
using System.Text.Json;
using Cocona;
using GitSharp.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class CommitCommands(ILogger<CommitCommands> logger)
{
    [Command(nameof(Commit))]
    public async Task Commit([Argument] string message)
    {
        logger.LogInformation("Committing with message: {Message}", message);
    }

    public async Task Status()
    {
        
    }

    public async Task GetByHash([Argument] string hash)
    {
        var path = $".gitsharp/objects/{hash[..2]}/{hash[2..]}";
        if (!File.Exists(path))
        {
            logger.LogInformation("Object not found");
            return;
        }

        await using var sourceStream = File.OpenRead(path);
        
        await using var zlibStream = new ZLibStream(sourceStream, CompressionMode.Decompress);
        using var reader = new StreamReader(zlibStream);

        var result = await reader.ReadToEndAsync();
        Console.WriteLine(result);
    }
}