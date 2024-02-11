using System.Data;
using System.Reflection;
using System.Text;

using Frank.DataStorage.Abstractions;

using Microsoft.Data.Sqlite;

namespace Frank.DataStorage.Sqlite;

public class SqliteRepository<T> : IRepository<T> where T : class, IKeyed, new()
{
    private readonly ISqliteClient _sqliteClient;

    public SqliteRepository(ISqliteClient sqliteClient)
    {
        _sqliteClient = sqliteClient;
        _sqliteClient.EnsureTableExistsAsync<T>().GetAwaiter().GetResult();
    }

    public IQueryable<T?> GetAll()
    {
        var entities = _sqliteClient.RunQueryAsync<T>($"SELECT * FROM {typeof(T).Name}").Result;
        
        if (entities.Rows.Count == 0)
            return new List<T?>().AsQueryable();
        
        var result = new List<T>();
        
        foreach (DataRow row in entities.Rows)
        {
            var entity = new T();
            foreach (var property in entity.GetType().GetProperties())
            {
                var fieldName = property.Name;
                var fieldType = property.PropertyType;
                var fieldExists = entities.Columns.Contains(fieldName);

                if (!fieldExists)
                    continue;

                var index = entities.Columns.IndexOf(fieldName);
                var value = row[index];

                if (value == DBNull.Value)
                    continue;

                entity.SetPropertyValue(fieldType, property, value);
            }
            result.Add(entity);
        }
        
        return result.AsQueryable();
    }

    public Task AddAsync(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var commandBuilder = new StringBuilder($"INSERT INTO {typeof(T).Name} (");
        foreach (var property in properties)
        {
            commandBuilder.Append($"{property.Name}, ");
        }
        commandBuilder.Remove(commandBuilder.Length - 2, 2);
        commandBuilder.Append(") VALUES (");
        foreach (var property in properties)
        {
            commandBuilder.Append($"{GetValue(entity, property)}, ");
        }
        commandBuilder.Remove(commandBuilder.Length - 2, 2);
        commandBuilder.Append(")");
        return _sqliteClient.RunNonQueryCommandAsync(commandBuilder.ToString());
    }

    private static object? GetValue(T entity, PropertyInfo property)
    {
        var value = property.GetValue(entity);
        return value switch
        {
            string => $"'{value}'",
            Guid => $"'{value}'",
            DateTime => $"'{value:yyyy-MM-dd HH:mm:ss}'",
            DateTimeOffset => $"'{value:yyyy-MM-dd HH:mm:ss zz00}'",
            TimeSpan => $"'{value:hh\\:mm\\:ss}'",
            _ => value
        };
    }

    public Task UpdateAsync(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var commandBuilder = new StringBuilder($"UPDATE {typeof(T).Name} SET ");
        foreach (var property in properties)
        {
            commandBuilder.Append($"{property.Name} = {property.GetValue(entity)}, ");
        }
        commandBuilder.Remove(commandBuilder.Length - 2, 2);
        commandBuilder.Append($" WHERE Id = '{entity.Id}'");
        return _sqliteClient.RunNonQueryCommandAsync(commandBuilder.ToString());
    }

    public async Task DeleteAsync(Guid id) => await _sqliteClient.RunNonQueryCommandAsync($"DELETE FROM {typeof(T).Name} WHERE Id = '{id}'");

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var query = $"SELECT * FROM {typeof(T).Name} WHERE Id = '{id}'";
        try
        {
            var dataTable = await _sqliteClient.RunQueryAsync<T>(query);
        
            if (dataTable.Rows.Count == 0)
                return null;
        
            var entity = new T();
        
            foreach (var property in entity.GetType().GetProperties())
            {
                var fieldName = property.Name;
                var fieldType = property.PropertyType;
                var fieldExists = dataTable.Columns.Contains(fieldName);

                if (!fieldExists)
                    continue;

                var index = dataTable.Columns.IndexOf(fieldName);
                var value = dataTable.Rows[0][index];

                if (value == DBNull.Value)
                    continue;

                entity.SetPropertyValue(fieldType, property, value);
            }
        
            return entity;
        }
        catch (SqliteException e)
        {
            throw new AggregateException($"Error running command: {query}", e);
        }
    }
}