using GitSharp.Test.Helpers;
using Shouldly;

namespace GitSharp.Test;

[Collection("Sequential")]
public class RunCoconaTest(GitSharpFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task RunCocona_Should_Not_Create_File_When_Not_Initiliazed()
    {
        // Arrange
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();
        
        // Act
        Directory.SetCurrentDirectory(DirectoryHelpers.TestDirectory);
        await Program.Main(["cat-file", "-p", "test.txt"]);
        
        // Assert
        File.Exists(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    ".gitsharp/objects/0a/0a9f2a6772942557ab5355d76af442f8f65e01"))
            .ShouldBeFalse();
    }
    
    
    [Fact]
    public async Task RunCocona_Should_Create_File_When_Initiliazed()
    {
        // Arrange
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();
        
        // Act
        Directory.SetCurrentDirectory(DirectoryHelpers.TestDirectory);
        await Program.Main(["init"]);
        await Program.Main(["hash-object", "-w", "test.txt"]);
        
        // Assert
        File.Exists(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    ".gitsharp/objects/0a/0a9f2a6772942557ab5355d76af442f8f65e01"))
            .ShouldBeTrue();
    }
}