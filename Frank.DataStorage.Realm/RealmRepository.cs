using Frank.DataStorage.Abstractions;

using Realms;

namespace Frank.DataStorage.Realm;

public class RealmRepository<T> : IRepository<T> where T : class, IKeyed, IRealmObject, new()
{
    private readonly RealmContext _context;

    public RealmRepository(RealmContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public IQueryable<T?> GetAll() => _context.Realm.All<T>().AsQueryable();

    /// <inheritdoc />
    public Task AddAsync(T entity) => Task.FromResult(_context.Realm.Write(() => _context.Realm.Add(entity)));

    /// <inheritdoc />
    public async Task UpdateAsync(T entity)
    {
        var existingEntity = await GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            return;
        }
        
        await _context.Realm.WriteAsync(() =>
        {
            _context.Realm.Remove(existingEntity);
            _context.Realm.Add(entity);
        });
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var existingEntity = await GetByIdAsync(id);
        if (existingEntity == null)
        {
            return;
        }
        
        await _context.Realm.WriteAsync(() => _context.Realm.Remove(existingEntity));
    }

    /// <inheritdoc />
    public Task<T?> GetByIdAsync(Guid id) => Task.FromResult(_context.Realm.All<T>().FirstOrDefault(x => x.Id == id));
}