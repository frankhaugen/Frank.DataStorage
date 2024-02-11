using System.Collections;

namespace Frank.DataStorage.Sqlite.Internals;

internal class BlobTypes : IEnumerable<Type>
{
    public IEnumerator<Type> GetEnumerator()
    {
        yield return typeof(byte[]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}