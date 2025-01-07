using System.IO.Compression;
using System.Text.Json;
using Cocona;
using GitSharp.Helpers;
using GitSharp.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class CommitCommands(ILogger<CommitCommands> logger, CommitBuilder commitBuilder)
{
    public async Task Commit([Argument] string message)
    {
        logger.LogInformation("Current hash: {CurrentHash}", HeadHelper.GetCurrentCommitHash());

        await commitBuilder.CreateCommit(message, true);
        
        logger.LogInformation("Commit created");
        logger.LogInformation("New hash: {NewHash}", HeadHelper.GetCurrentCommitHash());
    }
}