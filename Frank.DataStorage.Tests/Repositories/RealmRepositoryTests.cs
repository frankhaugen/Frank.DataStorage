using Frank.DataStorage.Tests.Shared;

using Xunit.Abstractions;

namespace Frank.DataStorage.Tests.Repositories;

/// <exclude/>
public class RealmRepositoryTests : DataStorageTestBase<ExampleClass>
{
    /// <inheritdoc />
    public RealmRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    /// <inheritdoc />
    public override StorageType GetStorageType()
    {
        return StorageType.Realm;
    }
}