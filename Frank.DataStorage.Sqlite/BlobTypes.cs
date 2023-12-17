using System.Collections;

namespace Frank.DataStorage.Sqlite;

internal class BlobTypes : IEnumerable<Type>
{
    public IEnumerator<Type> GetEnumerator()
    {
        yield return typeof(byte[]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}