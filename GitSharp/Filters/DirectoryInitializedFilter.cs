using Cocona.Filters;
using GitSharp.Services.Verifiers;

namespace GitSharp.Filters;

public class DirectoryInitializedFilter : ICommandFilter
{
    public async ValueTask<int> OnCommandExecutionAsync(
        CoconaCommandExecutingContext ctx,
        CommandExecutionDelegate next)
    {
        if (ctx.Command.Name == "init")
        {
            return await next(ctx);
        }

        if (!DirectoryVerifiers.FolderInitialized(Directory.GetCurrentDirectory()))
        {
            Console.WriteLine("GitSharp repository not initialized.");
            return 1;
        }

        return await next(ctx);
    }
}