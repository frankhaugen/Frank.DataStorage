using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Xml;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(XmlConnection));
        connectionString ??= Path.Combine(AppContext.BaseDirectory, "XmlData");
        services.AddSingletonIfNotRegistered<IOptions<XmlConnection>>(Options.Create(new XmlConnection { XmlDataFile = connectionString }));
        services.AddSingletonIfNotRegistered<XmlDataContext>();
        services.AddDataStorageRepository<XmlRepository<T>, T>();
        return services;
    }
}