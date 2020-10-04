using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Tests.Unit.Common.Logging
{
    public class FakeLoggerLine
    {
        public FakeLoggerLine(LogLevel level, EventId eventId, Exception? exception, string value)
        {
            Level = level;
            EventId = eventId;
            Exception = exception;
            Value = value;
        }

        public LogLevel Level { get; }

        public EventId EventId { get; }

        public Exception? Exception { get; }

        public string Value { get; }
    }
}