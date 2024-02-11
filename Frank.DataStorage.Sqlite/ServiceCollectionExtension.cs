using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteDataStorage<T>(this IServiceCollection services, IConfiguration configuration, string? databaseName = null) where T : class, IKeyed, new()
    {
        
        var connectionString = configuration.GetConnectionString(nameof(SqliteConnection));
        databaseName ??= "Storage.db";
        connectionString ??= $"Data Source={Path.Combine(AppContext.BaseDirectory, "SqliteData", databaseName)}";
        services.AddSingletonIfNotRegistered<IOptions<SqliteConnection>>(Options.Create(new SqliteConnection { ConnectionString = connectionString }));
        services.AddSingletonIfNotRegistered<ISqliteClient, SqliteClient>();
        services.AddDataStorageRepository<SqliteRepository<T>, T>();
        return services;
    }
}