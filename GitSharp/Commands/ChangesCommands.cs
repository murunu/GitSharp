using System.IO.Compression;
using System.Text.Json;
using Cocona;
using GitSharp.Extensions;
using GitSharp.Helpers;
using GitSharp.Models;
using GitSharp.Verifiers;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class ChangesCommands(
    ILogger<ChangesCommands> logger,
    TreeBuilder treeBuilder)
{
    public async Task Status()
    {
        if (!DirectoryVerifiers.FolderInitialized())
        {
            logger.LogError("Folder not initialized\nRun 'gitsharp init' first");
            return;
        }

        var newTree = await GetNewTree();
        var currentTree = await GetCurrentTree();

        await GetByHash(HeadHelper.GetCurrentCommitHash());

        Console.WriteLine("New tree hash:\t\t" + newTree.HashString);
        Console.WriteLine("Current tree hash:\t" + currentTree.HashString);

        Console.WriteLine("Length of new tree:\t" + newTree.Entries.Count);
        Console.WriteLine("Length of current tree:\t" + currentTree.Entries.Count);
        await Compare(newTree, currentTree);
    }

    private async Task Compare(Tree newTree, Tree currentTree)
    {
        // Get changes between keys of both trees
        currentTree.Entries.Keys.Except(newTree.Entries.Keys).ToList().ForEach(x => Console.WriteLine($"Removed: {x}"));
        
        foreach (var (file, model) in newTree.Entries)
        {
            if (currentTree.Entries.Values.Any(x => x.HashString == model.HashString)) 
                continue;
            
            if(currentTree.Entries.TryGetValue(file, out var entry))
            {
                Console.WriteLine($"Changed: {file}");
                
                
                Console.WriteLine("Old contents:");
                await GetByHash(entry.HashString);
                
                Console.WriteLine("New contents:");
                await GetByHash(model.HashString);
            }
            else
            {
                Console.WriteLine($"Added: {file}");
            }
        }
    }

    private async Task<Tree> GetNewTree()
    {
        var ignore = await IgnoreList.GetIgnoreList();

        var tree = new Tree();

        await treeBuilder.CreateTreeRecursive(Directory.GetCurrentDirectory(), tree, ignore);

        await tree.CreateHash();

        return tree.Flatten();
    }

    private async Task<Tree> GetCurrentTree()
    {
        var commitHash = HeadHelper.GetCurrentCommitHash();
        var commit = await HashHelper.GetByHash(commitHash);

        if (commit == null)
        {
            logger.LogError("Commit not found");
            return null;
        }

        var lines = commit.Split("\n");
        var hash = lines[0].Split(' ')[1];

        var tree = await Tree.FromHash(hash);
        return tree.Flatten();
    }

    public async Task GetByHash([Argument] string hash)
    {
        Console.WriteLine(await HashHelper.GetByHash(hash));
    }
}