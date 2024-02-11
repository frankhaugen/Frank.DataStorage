using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.LiteDb;

public class LiteDbRepository<T>(LiteDbDataContext liteDbDataContext) : IRepository<T>
    where T : class, IKeyed, new()
{
    public IQueryable<T?> GetAll() => liteDbDataContext.GetCollection<T>()
                                                       .FindAll()
                                                       .AsQueryable();

    public Task AddAsync(T entity) => Task.FromResult(liteDbDataContext.GetCollection<T>()
        .Insert(entity));

    public Task UpdateAsync(T entity) => Task.FromResult(liteDbDataContext.GetCollection<T>()
           .Update(entity));

    public Task DeleteAsync(Guid id) => Task.FromResult(liteDbDataContext.GetCollection<T>()
        .Delete(id));

    public Task<T?> GetByIdAsync(Guid id) => Task.FromResult(liteDbDataContext.GetCollection<T>()
        .FindById(id))!;
}