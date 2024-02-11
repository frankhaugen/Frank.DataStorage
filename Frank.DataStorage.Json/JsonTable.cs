using System.Text.Json;

namespace Frank.DataStorage.Json;

public class JsonTable<T> where T : class, new()
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1);
    private readonly string _folderPath;

    public JsonTable(string folderPath) => _folderPath = folderPath;

    private static string GetFilePath(string folderPath, Guid id) => Path.Combine(folderPath, $"{id}.json");

    public async Task<T?> GetAsync(Guid id)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var filePath = GetFilePath(_folderPath, id);
            if (!File.Exists(filePath))
                return null;
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task SaveAsync(Guid id, T entity)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var filePath = GetFilePath(_folderPath, id);
            var json = JsonSerializer.Serialize(entity);
            await File.WriteAllTextAsync(filePath, json);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async IAsyncEnumerable<T?> GetAllAsync()
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            foreach (var filePath in Directory.EnumerateFiles(_folderPath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                yield return JsonSerializer.Deserialize<T>(json);
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            File.Delete(GetFilePath(_folderPath, id));
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}