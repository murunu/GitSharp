using System.Security.Cryptography;
using GitSharp.Services.Base;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GitSharp.Services.Implementations;

internal class Sha1HashObjectService(
    IDirectoryService directoryService,
    IFileSystemService fileSystemService,
    ILogger<Sha1HashObjectService> logger) : HashObjectServiceBase(directoryService, fileSystemService, logger)
{
    protected override async Task<string> HashDataAsync(Stream stream)
    {
        using var sha1 = SHA1.Create();
        return Convert.ToHexString(await sha1.ComputeHashAsync(stream)).ToLower();
    }
}