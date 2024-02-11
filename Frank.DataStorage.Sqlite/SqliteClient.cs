using System.Data;

using Frank.DataStorage.Abstractions;
using Frank.DataStorage.Sqlite.Internals;
using Frank.Reflection;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Frank.DataStorage.Sqlite;

public class SqliteClient : ISqliteClient
{
    private readonly Microsoft.Data.Sqlite.SqliteConnection _connection;
    private readonly SqliteTypeMapper _sqliteTypeMapper = new();
    private bool _disposed;

    public SqliteClient(IOptions<SqliteConnection> options)
    {
        _connection = new(options.Value.ConnectionString ?? throw new InvalidOperationException("Connection string is not set."));
        
        var databaseFilePath = _connection.DataSource;
        
        if (string.IsNullOrWhiteSpace(databaseFilePath))
            throw new InvalidOperationException("Database file path is not set.");
        
        var databaseDirectory = Path.GetDirectoryName(databaseFilePath);
        
        if (string.IsNullOrWhiteSpace(databaseDirectory))
            throw new InvalidOperationException("Database directory is not set.");
        
        if (!Directory.Exists(databaseDirectory))
            Directory.CreateDirectory(databaseDirectory);
    }

    private static string GetTableName<T>() where T : class, IKeyed, new() => typeof(T).GetDisplayName();
    
    public async Task<T> RunQueryAsync<T>(string query, Func<SqliteDataReader, T> readerFunc) where T : class, IKeyed, new()
    {
        await using var command = new SqliteCommand(query, _connection);
        await _connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        var result = readerFunc(reader);
        await _connection.CloseAsync();
        return result;
    }

    /// <inheritdoc />
    public async Task<DataTable> RunQueryAsync<T>(string query) where T : class, IKeyed, new()
    {
        await using var command = new SqliteCommand(query, _connection);
        await _connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        var dataTable = new DataTable(GetTableName<T>());
        if (reader.HasRows)
            dataTable.Load(reader);
        await _connection.CloseAsync();

        return dataTable;
    }

    public async Task<int> RunNonQueryCommandAsync(string command)
    {
        try
        {
            await using var sqliteCommand = new SqliteCommand(command, _connection);
            await _connection.OpenAsync();
            var result = await sqliteCommand.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }
        catch (SqliteException e)
        {
            throw new AggregateException($"Error running command: {command}", e);
        }
    }
    
    public async Task EnsureTableExistsAsync<T>() where T : class, IKeyed, new()
    {
        var tableName = GetTableName<T>();
        var tableExistsQuery = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";

        await using var tableExistsCommand = new SqliteCommand(tableExistsQuery, _connection);
        await _connection.OpenAsync();
        var tableExists = await tableExistsCommand.ExecuteScalarAsync() != null;
        await _connection.CloseAsync();
        
        if (tableExists)
            return;

        var createTableStatement = _sqliteTypeMapper.CreateTableIfNotExistsStatement<T>();
        await using var createTableCommand = new SqliteCommand(createTableStatement, _connection);
        await _connection.OpenAsync();
        await createTableCommand.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _connection?.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}