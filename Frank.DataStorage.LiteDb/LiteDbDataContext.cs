using Frank.DataStorage.Abstractions;
using Frank.Reflection;

using LiteDB;

using Microsoft.Extensions.Options;

namespace Frank.DataStorage.LiteDb;

public class LiteDbDataContext : IDisposable
{
    private readonly ILiteDatabase _liteDatabase;

    public LiteDbDataContext(IOptions<LiteDbConnection> options)
    {
        var directory = new FileInfo(options.Value.LiteDbDataFile).Directory;
        if (directory is { Exists: false })
            directory.Create();
        _liteDatabase = new LiteDatabase(options.Value.LiteDbDataFile);
        _liteDatabase.UtcDate = true;
    }

    public ILiteCollection<T> GetCollection<T>() where T : class, IKeyed, new() => _liteDatabase.GetCollection<T>(typeof(T).GetDisplayName(), BsonAutoId.Guid);

    public void Dispose() => _liteDatabase.Dispose();
}