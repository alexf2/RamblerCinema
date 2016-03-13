using Rambler.Shared.Contract;

namespace Rambler.WindsorHelpers
{
    public static class BootstrapLogger
    {
        static BootstrapLogger()
        {
            Instance = NLog.LogManager.GetLogger(LoggerNames.Bootstrap);
        }

        public static NLog.Logger Instance { get; }
    }
}
