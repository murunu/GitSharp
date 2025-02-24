using Cocona;
using GitSharp.Services.Interfaces;
using GitSharp.Services.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class HashObjectCommands(IHashObjectService hashObjectService, ILogger<HashObjectCommands> logger)
{
    [Command("hash-object", Description = "Compute object ID and optionally creates a blob from a file")]
    public async Task<string> HashObject(
        [Option('w')] bool write, 
        [Option('t')] FileTypes? type, 
        [Argument("path")] string file)
    {
        logger.LogDebug("Hashing object from file {File}", file);

        var result = await hashObjectService.HashObject(file, type ?? FileTypes.Blob, write);
        
        logger.LogInformation("{Result}", result);

        return result;
    }
}