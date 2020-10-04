using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace IdentityProvider.Tests.Unit.Common.Logging
{
    public class FakeLogger : IFakeLogger
    {
        private readonly LogLevel[] _allowedLogLevels;
        private readonly object _locker = new object();
        private readonly List<FakeLoggerLine> _loggedLines = new List<FakeLoggerLine>(64);
        private readonly ILogger _nullLogger;
        private long _callsCount;

        public FakeLogger()
        {
            _nullLogger = NullLoggerFactory.Instance.CreateLogger(string.Empty);
            _allowedLogLevels = new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Critical,
                LogLevel.None
            };
        }

        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
        public FakeLogger(params LogLevel[] allowedLogLevels)
        {
            _nullLogger = NullLoggerFactory.Instance.CreateLogger(string.Empty);
            _allowedLogLevels = allowedLogLevels ?? new LogLevel[] { };
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _nullLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _allowedLogLevels.Contains(logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            lock (_locker)
            {
                var value = formatter(state, exception);
                _loggedLines.Add(new FakeLoggerLine(logLevel, eventId, exception, value));
                _callsCount += 1;
            }
        }

        public FakeLoggerState GetState()
        {
            lock (_locker)
            {
                var loggedLines = _loggedLines.ToArray();
                return new FakeLoggerState(_callsCount, loggedLines);
            }
        }
    }

    public class FakeLogger<T> : IFakeLogger<T>
    {
        private readonly LogLevel[] _allowedLogLevels;
        private readonly object _locker = new object();
        private readonly List<FakeLoggerLine> _loggedLines = new List<FakeLoggerLine>(64);
        private readonly ILogger _nullLogger;
        private long _callsCount;

        public FakeLogger()
        {
            _nullLogger = NullLoggerFactory.Instance.CreateLogger(string.Empty);
            _allowedLogLevels = new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Critical,
                LogLevel.None
            };
        }

        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
        public FakeLogger(params LogLevel[] allowedLogLevels)
        {
            _nullLogger = NullLoggerFactory.Instance.CreateLogger(string.Empty);
            _allowedLogLevels = allowedLogLevels ?? new LogLevel[] { };
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _nullLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _allowedLogLevels.Contains(logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            lock (_locker)
            {
                var value = formatter(state, exception);
                _loggedLines.Add(new FakeLoggerLine(logLevel, eventId, exception, value));
                _callsCount += 1;
            }
        }

        public FakeLoggerState GetState()
        {
            lock (_locker)
            {
                var loggedLines = _loggedLines.ToArray();
                return new FakeLoggerState(_callsCount, loggedLines);
            }
        }
    }
}