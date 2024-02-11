using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Json;

public class JsonRepository<T>(JsonContext context) : IRepository<T>
    where T : class, IKeyed, new()
{
    private readonly JsonTable<T> _table = context.GetTable<T>();

    public IQueryable<T?> GetAll() => _table.GetAllAsync().ToBlockingEnumerable().AsQueryable();

    public Task AddAsync(T entity) => _table.SaveAsync(entity.Id, entity);

    public Task UpdateAsync(T entity) => _table.SaveAsync(entity.Id, entity);

    public Task DeleteAsync(Guid id) => _table.DeleteAsync(id);

    public Task<T?> GetByIdAsync(Guid id) => _table.GetAsync(id);
}