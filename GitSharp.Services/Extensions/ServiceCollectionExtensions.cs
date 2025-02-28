using GitSharp.Serializers.Extensions;
using GitSharp.Services.Configuration;
using GitSharp.Services.Implementations;
using GitSharp.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitSharp.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitSharpServices(this IServiceCollection services,
        IConfiguration? configuration = null)
    {
        // Check if configuration is set.
        if (configuration is not null)
        {
            services.Configure<GitSharpConfiguration>(configuration.GetSection("GitSharp"));

            var gitSharpConfiguration = configuration.GetSection("GitSharp").Get<GitSharpConfiguration>();

            if (gitSharpConfiguration is { UseSha2: true })
            {
                services.AddSha2HashObjectService();
            }
        }
        else
        {
            services.Configure<GitSharpConfiguration>(_ => { });

            services.AddSha1HashObjectService();
        }

        services.AddSingleton<IDirectoryService, DirectoryService>();
        services.AddSingleton<IFileSystemService, FileSystemService>();
        services.AddSingleton<ICatFileService, CatFileService>();

        services.AddSerializers();
        return services;
    }

    private static void AddSha1HashObjectService(this IServiceCollection services)
    {
        services.AddSingleton<IHashObjectService, Sha1HashObjectService>();
    }

    private static void AddSha2HashObjectService(this IServiceCollection services)
    {
        services.AddSingleton<IHashObjectService, Sha2HashObjectService>();
    }

}