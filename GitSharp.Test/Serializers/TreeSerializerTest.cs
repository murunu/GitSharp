using GitSharp.Models;
using GitSharp.Serializers.Implementations;

namespace GitSharp.Test.Serializers;

[Trait("Category", "Serializers")]
public class TreeSerializerTest
{
    [Fact]
    public void TreeSerializer_Serialize_Should_Return_Success_When_Input_Is_Valid()
    {
        var tree = new Tree();
        tree.TreeEntries.Add(new TreeEntry
        {
            Name = "file.txt",
            Type = FileType.Blob,
            Mode = Mode.NormalFile,
            Hash = "1234567890abcdef1234567890abcdef12345678"
        });
        tree.TreeEntries.Add(new TreeEntry
        {
            Name = "folder",
            Type = FileType.Tree,
            Mode = Mode.Directory,
            Hash = "1234567890abcdef1234567890abcdef12345678"
        });
        
        var treeSerializer = new TreeSerializer();
        var serialized = treeSerializer.Serialize(tree);
        
        Assert.Equal("""
                     100644 blob 1234567890abcdef1234567890abcdef12345678	file.txt
                     040000 tree 1234567890abcdef1234567890abcdef12345678	folder
                     """, serialized);
    }
    
    [Fact]
    public void TreeSerializer_Deserialize_Should_Return_Success_When_Input_Is_Valid()
    {
        var treeSerializer = new TreeSerializer();
        var tree = treeSerializer.Deserialize(
            """
                 100644 blob 1234567890abcdef1234567890abcdef12345678	file .txt
                 040000 tree 1234567890abcdef1234567890abcdef12345678	folder
                 """);
        
        Assert.Equal(2, tree.TreeEntries.Count);
        
        Assert.Equal("file .txt", tree.TreeEntries[0].Name);
        Assert.Equal(FileType.Blob, tree.TreeEntries[0].Type);
        Assert.Equal(Mode.NormalFile, tree.TreeEntries[0].Mode);
        Assert.Equal("1234567890abcdef1234567890abcdef12345678", tree.TreeEntries[0].Hash);
        
        Assert.Equal("folder", tree.TreeEntries[1].Name);
        Assert.Equal(FileType.Tree, tree.TreeEntries[1].Type);
        Assert.Equal(Mode.Directory, tree.TreeEntries[1].Mode);
        Assert.Equal("1234567890abcdef1234567890abcdef12345678", tree.TreeEntries[1].Hash);
    }
    
    [Fact]
    public void TreeSerializer_Deserialize_Should_Return_Success_When_Input_Is_Empty()
    {
        var treeSerializer = new TreeSerializer();
        var tree = treeSerializer.Deserialize("");
        
        Assert.Empty(tree.TreeEntries);
    }
}