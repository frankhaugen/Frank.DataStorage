using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Csv;

public class CsvRepository<T>(CsvDocument<T> csvDocument) : IRepository<T>
    where T : class, IKeyed, new()
{
    public IQueryable<T?> GetAll() => csvDocument.AsQueryable();

    public void Add(T entity) => csvDocument.Add(entity);

    public void Update(T entity) => csvDocument.Update(entity);

    public void Delete(Guid id) => csvDocument.Delete(id);

    public T? GetById(Guid id) => csvDocument.GetById(id);
}