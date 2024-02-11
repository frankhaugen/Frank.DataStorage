using System.Diagnostics.CodeAnalysis;

using Frank.DataStorage.Abstractions;
using Frank.DataStorage.Csv;
using Frank.DataStorage.Json;
using Frank.DataStorage.LiteDb;
using Frank.DataStorage.MongoDb;
using Frank.DataStorage.Sqlite;
using Frank.DataStorage.Xml;
using Frank.Testing.TestBases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mongo2Go;

using Xunit.Abstractions;

namespace Frank.DataStorage.Tests.Shared;

public abstract class DataStorageTestBase<T> : HostApplicationTestBase, IAsyncDisposable where T : class, IKeyed, new()
{
    private MongoDbRunner? _runner;
    
    /// <inheritdoc />
    protected DataStorageTestBase(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        switch (GetStorageType())
        {
            case StorageType.Csv:
                builder.Services.AddCsvDataStorage<T>(builder.Configuration);
                break;
            case StorageType.Json:
                builder.Services.AddJsonDataStorage<T>(builder.Configuration);
                break;
            case StorageType.LiteDb:
                builder.Services.AddLiteDbDataStorage<T>(builder.Configuration);
                break;
            case StorageType.MongoDb:
                var dataPath = Path.Combine(AppContext.BaseDirectory, "MongoDbData");
                Directory.CreateDirectory(dataPath);
                _runner = MongoDbRunner.Start();
                var connectionString = _runner.ConnectionString;
                builder.Configuration["ConnectionStrings:MongoDbConnection"] = connectionString;
                builder.Services.AddMongoDbDataStorage<T>(builder.Configuration);
                break;
            case StorageType.Sqlite:
                builder.Services.AddSqliteDataStorage<T>(builder.Configuration);
                break;
            case StorageType.Xml:
                builder.Services.AddXmlDataStorage<T>(builder.Configuration);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return Task.CompletedTask;
    }
    
    public abstract StorageType GetStorageType();

    public IRepository<T> GetRepository<TImpl>() where TImpl : class, IRepository<T> => base.Services.GetServices<IRepository<T>>()?.FirstOrDefault(x => x.GetType() == typeof(TImpl)) as IRepository<T> ?? throw new InvalidOperationException($"No service of type {typeof(TImpl).Name} found.");

    /// <inheritdoc />
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    public new async ValueTask DisposeAsync()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (_runner is IAsyncDisposable runnerAsyncDisposable)
        {
            await runnerAsyncDisposable.DisposeAsync();
        }
        else
        {
            _runner?.Dispose();
        }
        
        await base.DisposeAsync();
    }
}

public enum StorageType
{
    Csv,
    Json,
    LiteDb,
    MongoDb,
    Sqlite,
    Xml
}