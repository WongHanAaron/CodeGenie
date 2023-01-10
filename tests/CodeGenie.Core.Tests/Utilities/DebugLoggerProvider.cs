using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Utilities
{
    public class DebugLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new DebugLogger(categoryName);
        }

        public void Dispose() { }

        public class DebugLogger : ILogger
        {
            string _categoryName;
            public DebugLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public IDisposable BeginScope<TState>(TState state)
                => default;

            public bool IsEnabled(LogLevel logLevel)
                => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                var message = $"{logLevel} - {_categoryName}:{formatter(state, exception)}";
                Debug.WriteLine(message);
                Console.WriteLine(message);
            }
        }
    }
}
