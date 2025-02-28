using GitSharp.Services.Configuration;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitSharp.Commands;

public class InitCommands(IOptions<GitSharpConfiguration> gitSharpConfiguration, IDirectoryService directoryService, ILogger<InitCommands> logger)
{
    private readonly GitSharpConfiguration _gitSharpConfiguration = gitSharpConfiguration.Value;

    public void Init()
    {
        // Create all folders.
        Directory.CreateDirectory(directoryService.GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "objects"));
        Directory.CreateDirectory(directoryService.GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs", "heads"));
        Directory.CreateDirectory(directoryService.GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs", "remotes"));
        Directory.CreateDirectory(directoryService.GetPath(_gitSharpConfiguration.GitSharpDirectoryName, "refs", "test"));

        logger.LogInformation("Initialized GitSharp repository");
    }
}