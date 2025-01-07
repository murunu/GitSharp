using System.Text;
using GitSharp.Models;
using GitSharp.Verifiers;
using Microsoft.Extensions.Logging;

namespace GitSharp.Helpers;

public class CommitBuilder(ILogger<CommitBuilder> logger, TreeBuilder treeBuilder)
{
    public async Task<Commit> CreateCommit(bool save = false)
    {
        if(!DirectoryVerifiers.FolderInitialized())
        {
            logger.LogError("Folder not initialized\nRun 'gitsharp init' first");
            throw new Exception("Folder not initialized");
        }
        
        var commit = new Commit()
        {
            Author = "Author",
            Committer = "Committer",
            Message = "Message",
            ParentHash = Convert.FromHexString(HeadHelper.GetCurrentCommitHash())
        };

        var tree = new Tree();

        await treeBuilder.CreateTreeRecursive(
            Directory.GetCurrentDirectory(),
            tree,
            await IgnoreList.GetIgnoreList(),
            save);

        await tree.CreateHash(true);
        
        await commit.CreateHash(tree, save);

        return commit;
    }
}