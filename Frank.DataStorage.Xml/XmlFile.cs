using System.Data;

namespace Frank.DataStorage.Xml;

public class XmlFile(FileInfo file)
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

    public void WriteXml(DataSet dataSet)
    {
        _lock.EnterWriteLock();
        try
        {
            dataSet.WriteXml(file.FullName, XmlWriteMode.IgnoreSchema);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public DataSet ReadXml()
    {
        _lock.EnterReadLock();
        try
        {
            var dataSet = new DataSet();
            file.Directory!.Create();
            if (!file.Exists)
                dataSet.WriteXml(file.FullName, XmlWriteMode.IgnoreSchema);

            dataSet.ReadXml(file.FullName, XmlReadMode.IgnoreSchema);
            return dataSet;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}