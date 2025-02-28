using GitSharp.Commands;
using GitSharp.Models;
using GitSharp.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace GitSharp.Test;

public class CatFileCommandTest(GitSharpFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task CatFileCommand_Should_Return_Content_When_Hash_Is_Provided()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        var hashObjectCommand = ServiceProvider.GetRequiredService<HashObjectCommands>();

        var hash = await hashObjectCommand.HashObject(true, FileType.Blob, "test.txt");

        // Act
        var catFileCommand = ServiceProvider.GetRequiredService<CatFileCommands>();

        var result = await catFileCommand.CatFile(hash, false, true, false);

        // Assert
        result.ShouldBe("Hello, World!");
    }

    [Fact]
    public async Task CatFileCommand_Should_Return_File_Type_When_Hash_Is_Provided()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        var hashObjectCommand = ServiceProvider.GetRequiredService<HashObjectCommands>();

        var hash = await hashObjectCommand.HashObject(true, FileType.Blob, "test.txt");

        // Act
        var catFileCommand = ServiceProvider.GetRequiredService<CatFileCommands>();

        var result = await catFileCommand.CatFile(hash, true, false, false);

        // Assert
        result.ShouldBe(FileType.Blob.ToString());
    }

    [Fact]
    public async Task CatFileCommand_Should_Return_File_Size_When_Hash_Is_Provided()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        // Initialize directory
        var initCommand = ServiceProvider.GetRequiredService<InitCommands>();
        initCommand.Init();

        var hashObjectCommand = ServiceProvider.GetRequiredService<HashObjectCommands>();

        var hash = await hashObjectCommand.HashObject(true, FileType.Blob, "test.txt");

        // Act
        var catFileCommand = ServiceProvider.GetRequiredService<CatFileCommands>();

        var result = await catFileCommand.CatFile(hash, false, false, true);

        // Assert
        result.ShouldBe(26.ToString());
    }

    [Fact]
    public async Task CatFileCommand_Should_Return_Error_When_No_Options_Provided()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        // Act
        var catFileCommand = ServiceProvider.GetRequiredService<CatFileCommands>();

        var result = await catFileCommand.CatFile("", false, false, false);

        // Assert
        result.ShouldBe("No option selected");
    }

    [Fact]
    public async Task CatFileCommand_Should_Throw_Exception_When_Invalid_Hash_Is_Provided()
    {
        // Arrange
        // Setup directory
        await DirectoryHelpers.SetupTestDirectoryWithTestFileAsync();

        // Act
        var catFileCommand = ServiceProvider.GetRequiredService<CatFileCommands>();

        // Act & Assert
        await catFileCommand.CatFile(
            "Invalid hash",
            false,
            true,
            false).ShouldThrowAsync<FileNotFoundException>();

    }
}