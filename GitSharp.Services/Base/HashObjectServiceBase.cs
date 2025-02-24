using GitSharp.Services.Interfaces;
using GitSharp.Services.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Services.Base;

public abstract class HashObjectServiceBase(
    IDirectoryService directoryService,
    IFileSystemService fileSystemService,
    ILogger<HashObjectServiceBase> logger
    ) : IHashObjectService
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
            await fileSystemService.WriteObjectAsync(hash, type, fileStream);
            
            logger.LogDebug("Object written to disk with hash: {Hash}", hash);
        }
        
        return hash;
    }
    
    protected abstract Task<string> HashDataAsync(Stream stream);
}