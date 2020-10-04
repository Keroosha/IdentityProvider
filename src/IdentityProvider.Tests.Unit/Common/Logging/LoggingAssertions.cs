using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace IdentityProvider.Tests.Unit.Common.Logging
{
    public static class LoggingAssertions
    {
        public static void CallExtensionsAndAssertLog(
            IFakeLogger logger,
            Action<ILogger> loggingExtension,
            LogLevel expectedLastMessageLogLevel,
            EventId expectedLastMessageEventId,
            string expectedLastMessageLogValue)
        {
            loggingExtension(logger);

            var state = logger.GetState();
            var lastMessage = state.LoggedLines.Last();

            Assert.AreEqual(1, state.LogCalls);
            Assert.AreEqual(expectedLastMessageLogLevel, lastMessage.Level);
            Assert.AreEqual(expectedLastMessageEventId, lastMessage.EventId);
            Assert.AreEqual(expectedLastMessageLogValue, lastMessage.Value);
        }

        public static void CallExtensionsAndAssertLog<T1>(
            IFakeLogger logger,
            Action<ILogger, T1> loggingExtension,
            T1 loggedValue1,
            LogLevel expectedLastMessageLogLevel,
            EventId expectedLastMessageEventId,
            string expectedLastMessageLogValue)
        {
            loggingExtension(logger, loggedValue1);

            var state = logger.GetState();
            var lastMessage = state.LoggedLines.Last();

            Assert.AreEqual(1, state.LogCalls);
            Assert.AreEqual(expectedLastMessageLogLevel, lastMessage.Level);
            Assert.AreEqual(expectedLastMessageEventId, lastMessage.EventId);
            Assert.AreEqual(expectedLastMessageLogValue, lastMessage.Value);
        }

        public static void CallExtensionsAndAssertLog<T1, T2>(
            IFakeLogger logger,
            Action<ILogger, T1, T2> loggingExtension,
            T1 loggedValue1,
            T2 loggedValue2,
            LogLevel expectedLastMessageLogLevel,
            EventId expectedLastMessageEventId,
            string expectedLastMessageLogValue)
        {
            loggingExtension(logger, loggedValue1, loggedValue2);

            var state = logger.GetState();
            var lastMessage = state.LoggedLines.Last();

            Assert.AreEqual(1, state.LogCalls);
            Assert.AreEqual(expectedLastMessageLogLevel, lastMessage.Level);
            Assert.AreEqual(expectedLastMessageEventId, lastMessage.EventId);
            Assert.AreEqual(expectedLastMessageLogValue, lastMessage.Value);
        }

        public static void AssertLog(
            IFakeLogger logger,
            int logMessageIndex,
            long expectedCallsCount,
            LogLevel expectedLastMessageLogLevel,
            EventId expectedLastMessageEventId,
            string expectedLastMessageLogValue)
        {
            var state = logger.GetState();
            var lastMessage = state.LoggedLines[logMessageIndex];

            Assert.AreEqual(expectedCallsCount, state.LogCalls);
            Assert.AreEqual(expectedLastMessageLogLevel, lastMessage.Level);
            Assert.AreEqual(expectedLastMessageEventId, lastMessage.EventId);
            Assert.AreEqual(expectedLastMessageLogValue, lastMessage.Value);
        }
    }
}