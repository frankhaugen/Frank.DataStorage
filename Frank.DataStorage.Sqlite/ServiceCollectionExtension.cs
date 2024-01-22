using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        services.Configure<SqliteConnection>(configuration.GetSection(nameof(SqliteConnection)));
        services.AddSingleton<ISqliteClient, SqliteClient>();
        services.AddSingleton<IRepository<T>, SqliteRepository<T>>();
        return services;
    }
}

public class SqliteConnection
{
    public string? ConnectionString { get; set; }
}