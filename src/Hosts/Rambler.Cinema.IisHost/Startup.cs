using Microsoft.Owin;
using Owin;
using Rambler.Cinema.Core.Contract;
using Rambler.Cinema.OwinService.Extensions;
using Rambler.Shared.Contract;
using Rambler.WebApiHelpers;
using Rambler.WindsorHelpers;

[assembly: OwinStartup(typeof(Rambler.Cinema.IisHost.Startup))]

namespace Rambler.Cinema.IisHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BootstrapLogger.Instance.Debug("Iis host is starting...");

            var container = WindsorConfig.Configure();
            BootstrapLogger.Instance.Debug("IoC configured");
            BootstrapLogger.Instance.Debug("Configuring Cinema service Owin middleware");
            app.Map("/api", bld =>
            {                
                bld.UseRamblerCinema(new RamblerCinemaServiceOptions(new WindsorDependencyResolver(container))
                {
                    ExceptionHandler = new WebApiExceptionHandler(),
                    ExceptionLogger = new WebApiExceptionLogger(container.Resolve<IExtendedLoggerFactory>().CreateExt(LoggerNames.UnhandledExc))
                });
            });
            BootstrapLogger.Instance.Debug("Cinema service was configured");                       
        }
    }
}
