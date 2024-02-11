using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.LiteDb;

public class LiteDbRepository<T>(LiteDbDataContext liteDbDataContext) : IRepository<T>
    where T : class, IKeyed, new()
{
    public IQueryable<T?> GetAll() => liteDbDataContext.GetCollection<T>()
                                                       .FindAll()
                                                       .AsQueryable();

    public void Add(T entity) => liteDbDataContext.GetCollection<T>()
                                                   .Insert(entity);

    public void Update(T entity) => liteDbDataContext.GetCollection<T>()
                                                      .Update(entity);

    public void Delete(Guid id) => liteDbDataContext.GetCollection<T>()
                                                     .Delete(id);

    public T? GetById(Guid id) => liteDbDataContext.GetCollection<T>()
                                                    .FindById(id);
}