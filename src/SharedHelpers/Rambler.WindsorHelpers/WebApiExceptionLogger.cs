using System.Web.Http.ExceptionHandling;
using Rambler.WebApiHelpers;

namespace Rambler.WindsorHelpers
{
    public sealed class WebApiExceptionLogger: ExceptionLogger
    {
        readonly IExtendedLogger _logger;

        public WebApiExceptionLogger (IExtendedLogger logger)
        {
            _logger = logger;
        }        

        public override void Log (ExceptionLoggerContext context)
        {
            var guidStr = context.Exception.Data.AssignGuid();
            _logger.Error($"Unhandled exception {guidStr}", context.Exception);
            //_logger.Error($"Unhandled exception", context.Exception);
        }
    }
    
}
