namespace Frank.DataStorage.Abstractions;

/// <summary>
/// Represents an entity with a unique identifier.
/// </summary>
public interface IKeyed
{
    /// <summary>
    /// Gets or sets the unique identifier for the property.
    /// </summary>
    Guid Id { get; set; }
}