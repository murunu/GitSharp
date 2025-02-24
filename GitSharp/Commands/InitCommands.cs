using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class InitCommands(ILogger<InitCommands> logger)
{
    public async Task Init()
    {
        // Create all folders.
        Directory.CreateDirectory(".gitsharp/objects");
        Directory.CreateDirectory(".gitsharp/refs/heads");
        Directory.CreateDirectory(".gitsharp/refs/remotes");
        Directory.CreateDirectory(".gitsharp/refs/tags");
        
        logger.LogInformation("Initialized GitSharp repository");
    }
}