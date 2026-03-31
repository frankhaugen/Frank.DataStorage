using Realms;

namespace Frank.DataStorage.Realm;

public class RealmContext
{
    private readonly RealmConnection _connection;

    public RealmContext(RealmConnection connection)
    {
        _connection = connection;
        Realm = Realms.Realm.GetInstance(new RealmConfiguration(_connection.DatabasePath));
    }

    public Realms.Realm Realm { get; }
}