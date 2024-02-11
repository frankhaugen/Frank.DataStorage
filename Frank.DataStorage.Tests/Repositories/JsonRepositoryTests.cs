using FluentAssertions;

using Frank.DataStorage.Json;
using Frank.DataStorage.Tests.Shared;

using Xunit.Abstractions;

namespace Frank.DataStorage.Tests.Repositories;

/// <exclude/>
public class JsonRepositoryTests : DataStorageTestBase<ExampleClass>
{
    public JsonRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    /// <inheritdoc />
    public override StorageType GetStorageType() => StorageType.Json;
    
    [Fact]
    public async Task RunTests()
    {
        // Arrange
        // Here we are assuming that JsonTable and JsonContext classes are something like this:
        var repository = GetRepository<JsonRepository<ExampleClass>>();

        var testData1 = new ExampleClass { Id = Guid.NewGuid(), SomeData = "Test1", DateTime = new DateTime(new DateOnly(2021, 1, 1), new TimeOnly(), DateTimeKind.Utc), DateTimeOffset = new DateTimeOffset(2021, 1, 1, 1, 1, 1, TimeSpan.Zero), TimeSpan = new TimeSpan(1, 2, 3), Boolean = true };
        var testData2 = new ExampleClass { Id = Guid.NewGuid(), SomeData = "Test2", DateTime = new DateTime(new DateOnly(2021, 1, 1), new TimeOnly(), DateTimeKind.Utc), DateTimeOffset = new DateTimeOffset(2021, 1, 1, 1, 1, 1, TimeSpan.Zero), TimeSpan = new TimeSpan(1, 2, 3), Boolean = true };

        // Act: Add some items to the repository
        await repository.AddAsync(testData1);
        await repository.AddAsync(testData2);

        // Assert
        // Check that the items exist in the repository
        var item1 = await repository.GetByIdAsync(testData1.Id);
        item1.Should()
             .NotBeNull();
        item1.Should()
             .BeEquivalentTo(testData1);

        var item2 = await repository.GetByIdAsync(testData2.Id);
        item2.Should()
             .NotBeNull();
        item2.Should()
             .BeEquivalentTo(testData2);

        // Act: Remove an item from the repository
        await repository.DeleteAsync(testData1.Id);

        // Assert
        // Check that the item does not exist in the repository
        item1 = await repository.GetByIdAsync(testData1.Id);
        item1.Should()
             .BeNull();
    }
}