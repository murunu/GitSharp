using GitSharp.Extensions;
using GitSharp.Models;
using GitSharp.Verifiers;
using Microsoft.Extensions.Logging;

namespace GitSharp.Commands;

public class TreeCommands(ILogger<TreeCommands> logger)
{
    public async Task CreateTree()
    {
        if(!DirectoryVerifiers.FolderInitialized())
        {
            logger.LogError("Folder not initialized\nRun 'gitsharp init' first");
            return;
        }

        var ignore = await GetIgnoreList();
        
        var tree = new Tree();
        
        await CreateTreeRecursive(Directory.GetCurrentDirectory(), tree, ignore);
        
        await tree.CreateHash();
        logger.LogInformation("Created root tree with hash: {TreeHashString}", tree.HashString);
    }

    private async Task CreateTreeRecursive(string path, Tree currentTree, List<string> ignore)
    {
        foreach (var file in path.GetFiles(ignore))
        {
            var blob = new Blob();
            await blob.CreateHash(File.OpenRead(file));

            currentTree.AddEntry(blob, Path.GetRelativePath(path, file));

            logger.LogInformation("Created blob for file {File} with hash: {BlobHashString}", file, blob.HashString);
        }
        
        foreach (var directory in path.GetDirectories(ignore))
        {
            var tree = new Tree();
            await CreateTreeRecursive(directory, tree, ignore);
            await tree.CreateHash();
            
            currentTree.AddEntry(tree, Path.GetRelativePath(path, directory));
            logger.LogInformation("Created tree for directory {Directory} with hash: {TreeHashString}", directory, tree.HashString);
        }
    }
    
    private static async Task<List<string>> GetIgnoreList()
        =>
        [
            ".gitsharp/",
            ..(await File.ReadAllLinesAsync(".gitsharpignore"))
            .Where(x => !x.StartsWith('#') && !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
        ];
}