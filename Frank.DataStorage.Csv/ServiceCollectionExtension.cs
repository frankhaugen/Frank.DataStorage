using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Csv;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCsvDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(CsvConnection));
        connectionString ??= Path.Combine(AppContext.BaseDirectory, "CsvData");
        services.AddSingletonIfNotRegistered<IOptions<CsvConnection>>(Options.Create(new CsvConnection { CsvDataFolder = connectionString }));
        services.AddSingletonIfNotRegistered<CsvDocument<T>>();
        services.AddDataStorageRepository<CsvRepository<T>, T>();
        return services;
    }
}