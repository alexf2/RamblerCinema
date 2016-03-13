using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Infrastructure;
using Owin;
using Microsoft.Owin.Logging;
using Rambler.Cinema.Core.Contract;
using Rambler.Cinema.OwinService.Configuration;
using Rambler.Shared.Contract;


namespace Rambler.Cinema.OwinService.Extensions
{
    public static class CinemaServiceAppBuilderExtensions
    {
        public static void UseRamblerCinema (this IAppBuilder app, RamblerCinemaServiceOptions opt)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var configuration = new HttpConfiguration();

            //configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;

            app.UseDependencyResolver(configuration, opt.Resolver);

            var fac = opt.Resolver.GetService<ILoggerFactory>();
            app.SetLoggerFactory(fac);            

            var logger = fac.Create(LoggerNames.CinemaCore);
            logger.WriteInformation("Configuring Cinema Owin service");

            if (opt.ExceptionLogger != null)
                configuration.Services.Replace(typeof(IExceptionLogger), opt.ExceptionLogger);
            if (opt.ExceptionHandler != null)
                configuration.Services.Replace(typeof(IExceptionHandler), opt.ExceptionHandler);

            //adding conversions from Func to OwinMiddleWare and back
            SignatureConversions.AddConversions(app);

            logger.WriteInformation("Adding Cinema Owin service routes");
            RouteConfig.RegisterRoutes(configuration);
            logger.WriteInformation("Configuring WebApi");
            WebApiConfig.Configure(configuration);
            app.UseWebApi(configuration);

            app.UseStageMarker(PipelineStage.MapHandler);

            // clears out the OWIN logger factory so we don't recieve other hosting related logs
            app.Properties["server.LoggerFactory"] = null;

            logger.WriteInformation("Cinema Owin service was configured");
        }
    }
}
