using System.Security.Cryptography;
using System.Text;

namespace GitSharp.Models;

public class Tree : ModelBase
{
    private readonly Dictionary<string, ModelBase> _entries = [];

    public async Task CreateHash()
    {
        await using var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(
            string.Join(
                '\n', 
                _entries
                    .Select(pair => pair.Value.HashString + ' ' + pair.Key))));
        
        if (!SHA1.TryHashData(memoryStream.ToArray(), Hash, out _))
        {
            throw new Exception("Failed to create hash.");
        }
        
        await CreateHash(memoryStream);
    }
    
    public void AddEntry(ModelBase entry, string fileName)
    {
        _entries.Add(fileName, entry);
    }
}