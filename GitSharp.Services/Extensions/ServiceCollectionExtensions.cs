using GitSharp.Services.Implementations;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GitSharp.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitSharpServices(this IServiceCollection services)
    {
        services.AddSingleton<IHashObjectService, HashObjectService>();
        services.AddSingleton<IDirectoryService, DirectoryService>();
        return services;
    }
}