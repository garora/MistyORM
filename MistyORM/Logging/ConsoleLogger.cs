using System;

namespace MistyORM.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        public void Out(string Message)
        {
            Console.WriteLine(Message);
        }
    }
}
