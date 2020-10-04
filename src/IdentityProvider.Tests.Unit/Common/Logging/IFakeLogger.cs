using Microsoft.Extensions.Logging;

namespace IdentityProvider.Tests.Unit.Common.Logging
{
    public interface IFakeLogger : ILogger
    {
        public FakeLoggerState GetState();
    }

    public interface IFakeLogger<out T> : IFakeLogger, ILogger<T>
    {
    }
}