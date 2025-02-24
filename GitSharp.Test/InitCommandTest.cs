using GitSharp.Commands;
using GitSharp.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace GitSharp.Test;

public class InitCommandTest(GitSharpFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task InitCommand_Should_Initialize_GitSharp_Directory()
    {
        // Arrange
        // Setup directory
        DirectoryHelpers.SetupTestDirectory();

        // Act
        var initCommand = ServiceProvider.GetRequiredService<InitCommands>();

        await initCommand.Init();

        // Assert
        Directory.Exists(".gitsharp/objects").ShouldBeTrue();
        Directory.Exists(".gitsharp/refs/heads").ShouldBeTrue();
        Directory.Exists(".gitsharp/refs/remotes").ShouldBeTrue();
        Directory.Exists(".gitsharp/refs/tags").ShouldBeTrue();
    }
}