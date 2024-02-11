using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbDataStorage<T>(this IServiceCollection services, IConfiguration configuration, string? databaseName = null) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(MongoDbConnection));
        connectionString ??= "mongodb://localhost:27017";
        databaseName ??= "DataStorage";
        services.AddSingletonIfNotRegistered<IOptions<MongoDbConnection>>(Options.Create(new MongoDbConnection { ConnectionString = connectionString, DatabaseName = databaseName }));
        services.AddSingletonIfNotRegistered<MongoDbContext>();
        services.AddDataStorageRepository<MongoDbRepository<T>, T>();
        return services;
    }
}