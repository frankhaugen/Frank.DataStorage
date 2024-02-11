using System.Data;

using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Sqlite;

public interface ISqliteClient : IDisposable
{
    Task EnsureTableExistsAsync<T>() where T : class, IKeyed, new();
    Task<DataTable> RunQueryAsync<T>(string query) where T : class, IKeyed, new();
    
    Task<int> RunNonQueryCommandAsync(string command);
}