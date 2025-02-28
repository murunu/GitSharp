namespace GitSharp.Services.Models;

public class TreeEntry
{
    /// <summary>
    /// 
    /// </summary>
    public required Mode Mode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public required FileType Type { get; set; }
    
    /// <summary>
    /// The generated hash for the target file.
    /// </summary>
    public required string Hash { get; set; }
    
    /// <summary>
    /// The name of the target object.
    /// </summary>
    public required string Name { get; set; }
}