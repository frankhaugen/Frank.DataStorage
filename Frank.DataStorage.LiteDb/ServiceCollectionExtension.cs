using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.LiteDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiteDbDataStorage<T>(this IServiceCollection services, IConfiguration configuration, string? databaseName = null) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(LiteDbConnection));
        databaseName ??= "Storage.db";
        connectionString ??= Path.Combine(AppContext.BaseDirectory, "LiteDbData", databaseName);
        services.AddSingletonIfNotRegistered<IOptions<LiteDbConnection>>(Options.Create(new LiteDbConnection { LiteDbDataFile = connectionString }));
        services.AddSingletonIfNotRegistered<LiteDbDataContext>();
        services.AddDataStorageRepository<LiteDbRepository<T>, T>();
        return services;
    }
}