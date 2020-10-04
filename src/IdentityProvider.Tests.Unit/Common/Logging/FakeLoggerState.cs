namespace IdentityProvider.Tests.Unit.Common.Logging
{
    public class FakeLoggerState
    {
        public FakeLoggerState(long logCalls, FakeLoggerLine[] loggedLines)
        {
            LogCalls = logCalls;
            LoggedLines = loggedLines;
        }

        public long LogCalls { get; }

        public FakeLoggerLine[] LoggedLines { get; }
    }
}