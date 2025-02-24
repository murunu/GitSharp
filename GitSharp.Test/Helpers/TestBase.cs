namespace GitSharp.Test.Helpers;

public abstract class TestBase(GitSharpFixture fixture) : IClassFixture<GitSharpFixture>
{
    protected readonly IServiceProvider ServiceProvider = fixture.ServiceProvider;
    protected readonly DirectoryHelpers DirectoryHelpers = fixture.DirectoryHelpers;
}