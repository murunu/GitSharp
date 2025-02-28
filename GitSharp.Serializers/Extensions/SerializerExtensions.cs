using GitSharp.Models;
using GitSharp.Serializers.Implementations;
using GitSharp.Serializers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GitSharp.Serializers.Extensions;

public static class SerializerExtensions
{
    public static IServiceCollection AddSerializerServices(this IServiceCollection services)
    {
        services.AddSerializer<ITreeSerializer, TreeSerializer, Tree>();

        return services;
    }

    private static void AddSerializer<TInterface, TImplementation, TType>(this IServiceCollection services)
    where TInterface : class, ISerializer<TType>
    where TImplementation : class, TInterface
    {
        services.AddSingleton<TInterface, TImplementation>();

        services.AddSingleton<ISerializer<TType>>(
            serviceProvider => serviceProvider.GetRequiredService<TInterface>());
    }
}