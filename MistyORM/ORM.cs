using MistyORM.Server;

namespace MistyORM
{
    public sealed class ORM
    {
        public static DbServer CreateServerObject(DbServerInfo Info) => new DbServer(Info);
    }
}