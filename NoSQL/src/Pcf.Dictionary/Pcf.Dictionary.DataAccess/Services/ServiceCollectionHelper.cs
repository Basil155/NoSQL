using Microsoft.Extensions.DependencyInjection;
using Pcf.Dictionary.DataAccess.Repositories;
using Redis.OM;

namespace Pcf.Dictionary.DataAccess.Services;

public static class ServiceCollectionHelper
{
    public static IServiceCollection InstallRedis(this IServiceCollection services, string? redisConnectionString)
    {
        services.AddSingleton(typeof(RedisConnectionProvider),
            new RedisConnectionProvider(redisConnectionString ?? "redis://localhost:6389"));
        services.AddHostedService<IndexCreationService>();
        services.AddTransient<DictionaryElementRepository, DictionaryElementRepository>();

        return services;
    }
}