using System.Security.Cryptography;
using System.Text;
using GitSharp.Helpers;

namespace GitSharp.Models;

public class Tree : ModelBase
{
    public readonly Dictionary<string, ModelBase> Entries = [];

    public async Task CreateHash(bool save = false)
    {
        await using var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(
            string.Join(
                '\n', 
                Entries
                    .Select(pair => pair.Value.HashString + ' ' + pair.Key))));
        
        await CreateHash(memoryStream, save);
    }
    
    public void AddEntry(ModelBase entry, string fileName)
    {
        Entries.Add(fileName, entry);
    }
    
    public static async Task<Tree?> FromHash(string hash)
    {
        var tree = new Tree()
        {
            Hash = Convert.FromHexString(hash)
        };
        
        var result = await HashHelper.GetByHash(hash);
        
        if (result == null)
        {
            Console.WriteLine("Failed to get tree by hash");
            return tree;
        }

        var lines = result.Split('\n');
        
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var objectHash = parts[0];
            var fileName = string.Join("", parts[1..]);

            if (fileName.EndsWith(@"\") && await FromHash(objectHash) is { } subTree)
            {
                tree.AddEntry(subTree, fileName);
            }
            else if (await Blob.FromHash(objectHash) is { } blob)
            {
                tree.AddEntry(blob, fileName);
            }
        }
        
        return tree;
    }
    
    public Tree Flatten()
    {
        var tree = new Tree()
        {
            Hash = Hash
        };
        
        foreach (var (key, value) in Entries)
        {
            if (value is Blob blob)
            {
                tree.AddEntry(blob, key);
            }
            else if (value is Tree subTree)
            {
                foreach (var (subKey, subValue) in subTree.Flatten().Entries)
                {
                    tree.AddEntry(subValue, Path.Combine(key, subKey));
                }
                
                tree.AddEntry(subTree, key);
            }
        }
        
        return tree;
    }
}