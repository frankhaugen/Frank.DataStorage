using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.LiteDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiteDbDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(LiteDbConnection));
        connectionString ??= Path.Combine(AppContext.BaseDirectory, "LiteDbData", "Storage.db");
        services.AddSingleton<IOptions<LiteDbConnection>>(Options.Create(new LiteDbConnection { LiteDbDataFile = connectionString }));
        services.AddSingleton<LiteDbDataContext>();
        services.AddSingleton<IRepository<T>, LiteDbRepository<T>>();
        return services;
    }
}