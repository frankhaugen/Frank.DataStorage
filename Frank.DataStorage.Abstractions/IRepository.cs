namespace Frank.DataStorage.Abstractions;

/// <summary>
/// A generic repository interface for data access.
/// </summary>
public interface IRepository<T> where T : class, IKeyed, new()
{
    /// <summary>
    /// Retrieves all entities of type T from the data source.
    /// </summary>
    /// <typeparam name="T">The type of entities to retrieve.</typeparam>
    /// <returns>An IQueryable collection of entities.</returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// Adds a new entity of type T to the collection.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    void Add(T entity);

    /// <summary>
    /// Updates the specified entity in the database.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an item with the specified id.
    /// </summary>
    /// <param name="id">The unique identifier of the item to be deleted.</param>
    /// <remarks>
    /// The Delete method removes an item from the system based on its id.
    /// This operation is irreversible and cannot be undone.
    /// </remarks>
    void Delete(Guid id);

    /// <summary>
    /// Retrieves an object by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the object.</param>
    /// <returns>The object with the given identifier, or null if not found.</returns>
    T? GetById(Guid id);

    /// <summary>
    /// Saves the changes made to the database.
    /// </summary>
    void SaveChanges();
}