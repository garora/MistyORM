using MistyORM.Managers;
using MistyORM.Server;

namespace MistyORM
{
    public sealed class ORM
    {
        public static DbServer CreateServer(DbServerInfo Info)
        {
            Manager.Cache.Build();

            return new DbServer(Info);
        }
    }
}
