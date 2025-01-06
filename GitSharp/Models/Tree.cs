using System.Security.Cryptography;

namespace GitSharp.Models;

public class Tree : ModelBase
{
    public Tree()
    {
        Hash = SHA1.HashData(new MemoryStream(Guid.CreateVersion7().ToByteArray())) 
               ?? throw new Exception("Failed to create hash.");
    }
}