using GitSharp.Extensions;
using GitSharp.Models;
using Microsoft.Extensions.Logging;

namespace GitSharp.Helpers;

public class TreeBuilder(ILogger<TreeBuilder> logger)
{
    public async Task CreateTreeRecursive(string path, Tree currentTree, List<string> ignore, bool save = false)
    {
        foreach (var file in path.GetFiles(ignore))
        {
            var blob = new Blob();
            await blob.CreateHash(File.OpenRead(file), save);

            currentTree.AddEntry(blob, Path.GetRelativePath(path, file));

            logger.LogInformation("Created blob for file {File} with hash: {BlobHashString}", file, blob.HashString);
        }
        
        foreach (var directory in path.GetDirectories(ignore))
        {
            var tree = new Tree();
            await CreateTreeRecursive(directory, tree, ignore);
            await tree.CreateHash(save);
            
            currentTree.AddEntry(tree, Path.GetRelativePath(path, directory));
            logger.LogInformation("Created tree for directory {Directory} with hash: {TreeHashString}", directory, tree.HashString);
        }
    }
}