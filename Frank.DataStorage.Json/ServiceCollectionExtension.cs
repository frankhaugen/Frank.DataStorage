using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Json;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsonDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(JsonConnection));
        connectionString ??= Path.Combine(AppContext.BaseDirectory, "JsonData");
        services.AddSingletonIfNotRegistered<IOptions<JsonConnection>>(Options.Create(new JsonConnection { JsonDataFolder = connectionString }));
        services.AddSingletonIfNotRegistered<JsonContext>();
        services.AddDataStorageRepository<JsonRepository<T>, T>();
        return services;
    }
}