using GitSharp.Extensions;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace GitSharp.Test.Helpers;

public class GitSharpFixture
{
    public IServiceProvider ServiceProvider { get; }
    
    public DirectoryHelpers DirectoryHelpers { get; } = new();

    public GitSharpFixture()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));
        
        services.AddGitSharpCommands();
        services.RemoveAll<IDirectoryService>();

        var directoryServiceMock = new Mock<IDirectoryService>();
        
        directoryServiceMock.Setup(p => p.GetPath(It.IsAny<string[]>()))
            .Returns<string[]>(x => Path.Combine([DirectoryHelpers.TestDirectory, ..x]));
        
        directoryServiceMock.Setup(p => p.IsInitialized)
            .Returns(true);

        services.AddSingleton(directoryServiceMock.Object);
        
        ServiceProvider = services.BuildServiceProvider();
    }
}