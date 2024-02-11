using System.Text.Json;

namespace Frank.DataStorage.Json;

public class JsonTable<T>(string folderPath)
    where T : class, new()
{
    private readonly ReaderWriterLockSlim _lockSlim = new();

    public T? Get(Guid id)
    {
        _lockSlim.EnterReadLock();
        try
        {
            var filePath = GetFilePath(folderPath, id);
            if (!File.Exists(filePath))
            {
                return null;
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    public void Save(Guid id, T entity)
    {
        _lockSlim.EnterWriteLock();
        try
        {
            var filePath = GetFilePath(folderPath, id);
            var json = JsonSerializer.Serialize(entity);
            File.WriteAllText(filePath, json);
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    public IEnumerable<T?> GetAll()
    {
        _lockSlim.EnterReadLock();
        try
        {
            foreach (var filePath in Directory.EnumerateFiles(folderPath))
            {
                var json = File.ReadAllText(filePath);
                yield return JsonSerializer.Deserialize<T>(json);
            }
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    public void Delete(Guid id)
    {
        _lockSlim.EnterWriteLock();
        try
        {
            File.Delete(GetFilePath(folderPath, id));
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    private static string GetFilePath(string folderPath, Guid id) => Path.Combine(folderPath, $"{id}.json");
}