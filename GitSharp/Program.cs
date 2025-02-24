using Cocona;
using GitSharp.Extensions;
using GitSharp.Filters;
using GitSharp.Formatters;
using GitSharp.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace GitSharp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CoconaApp.CreateBuilder(args);

        builder.Logging
            .AddConsoleFormatter<CustomFormatter, SimpleConsoleFormatterOptions>();

        builder.Configuration.AddJsonFile("appsettings.json", optional: true);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile($"appsettings.{Environments.Development}.json", optional: true);
        }

        builder.Services.AddGitSharpServices(builder.Configuration);

        var app = builder.Build();

        app.UseFilter(new DirectoryInitializedFilter());

        app.AddGitSharpCommands();

        await app.RunAsync();
    }
}