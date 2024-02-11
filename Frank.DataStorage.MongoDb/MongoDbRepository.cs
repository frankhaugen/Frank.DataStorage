using Frank.DataStorage.Abstractions;

using MongoDB.Driver;

namespace Frank.DataStorage.MongoDb;

public class MongoDbRepository<T>(MongoDbContext context) : IRepository<T>
    where T : class, IKeyed, new()
{
    private readonly IMongoCollection<T> _collection = context.GetCollection<T>();

    public IQueryable<T?> GetAll() => _collection.AsQueryable();

    public Task AddAsync(T entity) => _collection.InsertOneAsync(entity);

    public Task UpdateAsync(T entity) => _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    public Task DeleteAsync(Guid id) => _collection.DeleteOneAsync(x => x.Id == id);

    public Task<T?> GetByIdAsync(Guid id) => Task.FromResult(_collection.Find(x => x.Id == id).FirstOrDefault())!;
}