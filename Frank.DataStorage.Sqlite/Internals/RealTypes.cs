using System.Collections;

namespace Frank.DataStorage.Sqlite.Internals;

internal class RealTypes : IEnumerable<Type>
{
    public IEnumerator<Type> GetEnumerator()
    {
        yield return typeof(double);
        yield return typeof(double?);
        yield return typeof(float);
        yield return typeof(float?);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}