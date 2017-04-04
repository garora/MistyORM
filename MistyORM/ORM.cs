using MistyORM.Server;

namespace MistyORM
{
    public sealed class ORM
    {
        public static DbServer CreateServer(DbServerInfo Info) => new DbServer(Info);
    }
}
