using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Rambler.WebApiHelpers;

namespace Rambler.WindsorHelpers
{
    public class ExceptionLogger: IExceptionLogger
    {
        readonly IExtendedLogger _logger;

        public ExceptionLogger (IExtendedLogger logger)
        {
            _logger = logger;
        }

        public Task LogAsync (ExceptionLoggerContext context, CancellationToken cancellationToken)
        {            
            _logger.Error("Unhandled exception", context.Exception);

            return Task.FromResult<object>(null);
        }
    }
}
