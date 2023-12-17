using Frank.DataStorage.Abstractions;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(SqliteConnection));
        connectionString ??= "Data Source=Frank.Libraries.DataStorage.db;";
        services.AddSingleton<IOptions<SqliteConnection>>(Options.Create<SqliteConnection>(new SqliteConnection { ConnectionString = connectionString }));
        services.AddSingleton<ISqliteClient, SqliteClient>();
        services.AddSingleton<IRepository<T>, SqliteRepository<T>>();
        return services;
    }
}