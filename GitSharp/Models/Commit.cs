using System.Text;
using GitSharp.Helpers;

namespace GitSharp.Models;

public class Commit : ModelBase
{ 
    public byte[]? ParentHash { get; init; }
    
    public string? ParentHashString => ParentHash is null ? null : Convert.ToHexString(ParentHash);

    public string? Author { get; init; }
    
    public string? Committer { get; init; }
    
    public string? Message { get; init; }

    public DateTime CommitDate { get; } = DateTime.UtcNow;
    
    public async Task CreateHash(Tree tree, bool save = false)
    {
        await using var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(
            $"tree {tree.HashString}\n" +
            (ParentHash is null ? "" : $"parent {ParentHashString}\n") +
            $"author {Author} {CommitDate:O}\n" +
            $"committer {Committer} {CommitDate:O}\n\n" +
            $"message Message"));
        
        await CreateHash(memoryStream, save);

        HeadHelper.SetCommit(HashString);
    }
}