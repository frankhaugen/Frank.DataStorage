namespace Frank.DataStorage.Abstractions;

public interface IKeyed
{
    Guid Id { get; set; }
}