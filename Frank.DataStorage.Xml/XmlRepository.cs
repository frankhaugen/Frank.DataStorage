using System.Data;

using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Xml;

public class XmlRepository<T> : IRepository<T> where T : class, IKeyed, new()
{
    private readonly XmlDataContext _dataContext;
    private readonly DataTable _table;

    public XmlRepository(XmlDataContext dataContext)
    {
        _dataContext = dataContext;
        _table = _dataContext.GetTable<T>();
        _dataContext.SaveChanges();
    }

    public IQueryable<T?> GetAll() => _table.Rows.Cast<DataRow>()
                                           .Select(GetEntityFromDataRow)
                                           .AsQueryable();

    public Task AddAsync(T entity)
    {
        var row = _table.NewRow();
        SetDataRowFromEntity(row, entity);
        _table.Rows.Add(row);
        _dataContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity)
    {
        var row = _table.Rows.Find(entity.Id);
        SetDataRowFromEntity(row, entity);
        _dataContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var row = _table.Rows.Find(id);
        row?.Delete();
        _dataContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        var row = _table.Rows.Find(id);
        return Task.FromResult(row == null ? null : GetEntityFromDataRow(row))!;
    }

    private static T GetEntityFromDataRow(DataRow row)
    {
        var entity = new T();
        foreach (DataColumn column in row.Table.Columns)
        {
            var property = typeof(T).GetProperty(column.ColumnName);
            if (property != null)
            {
                property.SetValue(entity, row[column.ColumnName], null);
            }
        }

        return entity;
    }

    private static void SetDataRowFromEntity(DataRow? row, T entity)
    {
        if (row == null)
        {
            return;
        }

        foreach (DataColumn column in row.Table.Columns)
        {
            var property = typeof(T).GetProperty(column.ColumnName);
            if (property != null)
            {
                row[column.ColumnName] = property.GetValue(entity, null);
            }
        }
    }
}