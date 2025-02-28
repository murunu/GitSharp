using System.Security.Cryptography;
using GitSharp.Services.Base;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GitSharp.Services.Implementations;

internal class Sha2HashObjectService(
    IDirectoryService directoryService,
    IFileSystemService fileSystemService,
    ILogger<Sha2HashObjectService> logger) : HashObjectServiceBase(directoryService, fileSystemService, logger)
{
    protected override async Task<string> HashDataAsync(Stream stream)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToHexString(await sha256.ComputeHashAsync(stream)).ToLower();
    }
}