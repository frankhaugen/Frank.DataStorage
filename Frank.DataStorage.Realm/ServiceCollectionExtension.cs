using Frank.DataStorage.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Realms;

namespace Frank.DataStorage.Realm;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRealmDataStorage<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IKeyed, IRealmObject, new()
    {
        var connectionString = configuration.GetConnectionString(nameof(RealmConnection));
        connectionString ??= "DataStorage.realm";
        services.AddSingletonIfNotRegistered<IOptions<RealmConnection>>(Options.Create(new RealmConnection { DatabasePath = connectionString }));
        services.AddSingletonIfNotRegistered<RealmContext>();
        services.AddDataStorageRepository<RealmRepository<T>, T>();
        return services;
    }
    
    public static IServiceCollection AddRealmDataStorage<T>(this IServiceCollection services, string databasePath) where T : class, IKeyed, IRealmObject, new()
    {
        services.AddSingletonIfNotRegistered<IOptions<RealmConnection>>(Options.Create(new RealmConnection { DatabasePath = databasePath }));
        services.AddSingletonIfNotRegistered<RealmContext>();
        services.AddDataStorageRepository<RealmRepository<T>, T>();
        return services;
    }
}