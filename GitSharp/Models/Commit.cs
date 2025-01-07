namespace GitSharp.Models;

public class Commit : ModelBase
{ 
    public byte[]? ParentHash { get; protected set; }
    
    public string? ParentHashString => ParentHash is null ? null : Convert.ToHexString(ParentHash);

}