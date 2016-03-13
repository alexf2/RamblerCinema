using Microsoft.Owin.Logging;

namespace Rambler.WebApiHelpers
{
    public interface IExtendedLoggerFactory : ILoggerFactory
    {
        IExtendedLogger CreateExt (string name);
    }
}
