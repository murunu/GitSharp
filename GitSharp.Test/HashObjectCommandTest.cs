using GitSharp.Commands;
using GitSharp.Models;
using GitSharp.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace GitSharp.Test;

public class HashObjectCommandTest(GitSharpFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HashObjectCommand_Should_Create_Object_File()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        // Act
        var hashObjectCommand = ServiceProvider.GetRequiredService<HashObjectCommands>();

        var result = await hashObjectCommand.HashObject(true, FileType.Blob, "test.txt");

        // Assert
        result.ShouldBe("0a0a9f2a6772942557ab5355d76af442f8f65e01");
        File.Exists(
                Path.Combine(
                    DirectoryHelpers.TestDirectory,
                    ".gitsharp/objects/0a/0a9f2a6772942557ab5355d76af442f8f65e01"))
            .ShouldBeTrue();
    }

    [Fact]
    public async Task HashObjectCommand_Should_Throw_When_File_Does_Not_Exist()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        // Act
        var hashObjectCommand = ServiceProvider.GetRequiredService<HashObjectCommands>();

        // Act & Assert
        await hashObjectCommand.HashObject(
            true,
            FileType.Blob,
            "non-existing file.txt")
            .ShouldThrowAsync<FileNotFoundException>();
    }
}