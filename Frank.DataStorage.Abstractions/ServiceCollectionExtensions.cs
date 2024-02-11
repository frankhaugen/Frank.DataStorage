using Microsoft.Extensions.DependencyInjection;

namespace Frank.DataStorage.Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataStorageRepository<TRepo, T>(this IServiceCollection services) where TRepo : class, IRepository<T> where T : class, IKeyed, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        if (services.Any(x => x.ServiceType == typeof(IRepository<T>)))
            throw new InvalidOperationException($"Repository for {typeof(T).Name} already registered.");
        services.AddSingleton<IRepository<T>, TRepo>();
        return services;
    }
    
    public static IServiceCollection AddSingletonIfNotRegistered<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        if (services.Any(x => x.ServiceType == typeof(TService)))
            return services;
        services.AddSingleton<TService, TImplementation>();
        return services;
    }
    
    public static IServiceCollection AddSingletonIfNotRegistered<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        if (services.Any(x => x.ServiceType == typeof(TService)))
            return services;
        services.AddSingleton(implementationInstance);
        return services;
    }
    
    public static IServiceCollection AddSingletonIfNotRegistered<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        if (services.Any(x => x.ServiceType == typeof(TService)))
            return services;
        services.AddSingleton(implementationFactory);
        return services;
    }
    
    public static IServiceCollection AddSingletonIfNotRegistered<TService>(this IServiceCollection services) where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        if (services.Any(x => x.ServiceType == typeof(TService)))
            return services;
        services.AddSingleton<TService>();
        return services;
    }
}