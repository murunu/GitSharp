using Cocona;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class CatFileCommands(IHashObjectService hashObjectService, ILogger<CatFileCommands> logger)
{
    public async Task<string> CatFile(
        [Argument] string hash,
        [Option('t')] bool showType,
        [Option('p')] bool prettyPrint,
        [Option('s')] bool size)
    {
        if (showType)
        {
            var result = await hashObjectService.GetObjectTypeAsync(hash);
 
            logger.LogInformation("{Result}", result);

            return result.ToString();
        }

        if (prettyPrint)
        {
            var result = await hashObjectService.ReadObjectAsync(hash);

            logger.LogInformation("{Result}", result);

            return result;

        }

        if (size)
        {
            var result = await hashObjectService.GetSizeAsync(hash);
            
            logger.LogInformation("{Result}", result);

            return result.ToString();
        }

        logger.LogInformation("No option selected");
        
        return "No option selected";
    }
}