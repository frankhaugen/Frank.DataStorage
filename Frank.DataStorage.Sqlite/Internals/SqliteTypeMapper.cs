using System.Reflection;

using Frank.DataStorage.Abstractions;
using Frank.Reflection;

namespace Frank.DataStorage.Sqlite.Internals;

internal class SqliteTypeMapper
{
    private readonly SqliteTypeMappingDefinition _typeMappingDefinition = new();

    public string CreateTableIfNotExistsStatement<T>() where T : class, IKeyed, new()
    {
        var tableName = typeof(T).GetDisplayName();
        var properties = typeof(T).GetProperties();

        var columns = new List<string>();
        foreach (var property in properties)
        {
            columns.Add(CreateTableColumnDefinition(property));
        }

        var createTableStatement = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columns)});";
        return createTableStatement;
    }

    public string CreateTableColumnDefinition(PropertyInfo property)
    {
        var columnName = property.Name;
        var columnType = _typeMappingDefinition[property.PropertyType];
        var columnDefinition = $"{columnName} {columnType}";

        if (property.Name == nameof(IKeyed.Id))
        {
            columnDefinition += " PRIMARY KEY";
        }

        return columnDefinition;
    }
}