using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Csv;

public class CsvRepository<T>(CsvDocument<T> csvDocument) : IRepository<T>
    where T : class, IKeyed, new()
{
    public IQueryable<T?> GetAll() => csvDocument.AsQueryable();

    public Task AddAsync(T entity)
    {
        csvDocument.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity) => Task.FromResult(csvDocument.Update(entity));

    public Task DeleteAsync(Guid id) => csvDocument.DeleteAsync(id);

    public Task<T?> GetByIdAsync(Guid id) => Task.FromResult(csvDocument.GetById(id))!;
}