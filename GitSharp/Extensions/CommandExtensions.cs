using System.Diagnostics.CodeAnalysis;
using Cocona;
using GitSharp.Commands;
using GitSharp.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GitSharp.Extensions;

public static class CommandExtensions
{
    private static List<Type> Commands
        =>
        [
            typeof(HashObjectCommands),
            typeof(CatFileCommands),
            typeof(InitCommands)
        ];
    
    public static CoconaApp AddGitSharpCommands(this CoconaApp app)
    {
        foreach (var command in Commands)
        {
            app.AddCommands(command);
        }
        
        return app;
    }
    
    public static IServiceCollection AddGitSharpCommands(this IServiceCollection services)
    {
        foreach (var command in Commands)
        {
            services.AddSingleton(command);
        }

        services.AddGitSharpServices();

        return services;
    }
}