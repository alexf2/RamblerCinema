using Rambler.Shared.Contract;

namespace Rambler.WindsorHelpers
{
    public static class BootstrapLogger
    {
        static readonly NLog.Logger _logger;

        static BootstrapLogger()
        {
            _logger = NLog.LogManager.GetLogger(LoggerNames.Bootstrap);
        }

        public static NLog.Logger Instance
        {
            get { return _logger; }
        }
    }
}
