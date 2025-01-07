using GitSharp.Extensions;
using GitSharp.Helpers;
using GitSharp.Models;
using GitSharp.Verifiers;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class TreeCommands(ILogger<TreeCommands> logger,
    TreeBuilder treeBuilder,
    CommitBuilder commitBuilder)
{
    public async Task CreateTree()
    {
        // Create initial commit.
        await commitBuilder.CreateCommit("initial commit",true);
    }
}