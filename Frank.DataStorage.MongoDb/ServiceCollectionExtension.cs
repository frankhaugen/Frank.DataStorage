using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbDataStorage<T>(this IServiceCollection services, string databaseName, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(MongoDbConnection));
        connectionString ??= "mongodb://localhost:27017";
        services.AddSingleton<IOptions<MongoDbConnection>>(Options.Create(new MongoDbConnection { ConnectionString = connectionString, DatabaseName = databaseName }));
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<IRepository<T>, MongoDbRepository<T>>();
        return services;
    }
}