using System;

namespace MistyORM.Logging
{
    internal sealed class ConsoleLogger : ILogger
    {
        void ILogger.Out(string Message)
        {
            Console.WriteLine(Message);
        }
    }
}
