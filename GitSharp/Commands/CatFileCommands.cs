using Cocona;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class CatFileCommands(
    ICatFileService catFileService,
    IFileSystemService fileSystemService,
    ILogger<CatFileCommands> logger)
{
    public async Task<string> CatFile(
        [Argument(Description = "The name of the object to show.")] string hash,
        [Option('t', Description = "Instead of the content, show the object type identified by the hash.")] bool showType,
        [Option('p', Description = "Pretty-print the contents of the hash based on its type.")] bool prettyPrint,
        [Option('s', Description = "Instead of the content, show the object size identified by the hash")] bool size)
    {
        if (showType)
        {
            var result = await catFileService.GetObjectTypeAsync(hash);

            logger.LogInformation("{Result}", result);

            return result.ToString();
        }

        if (prettyPrint)
        {
            var result = await fileSystemService.ReadObjectAsync(hash);

            logger.LogInformation("{Result}", result);

            return result;
        }

        if (size)
        {
            var result = await catFileService.GetSizeAsync(hash);

            logger.LogInformation("{Result}", result);

            return result.ToString();
        }

        logger.LogInformation("No option selected");

        return "No option selected";
    }
}