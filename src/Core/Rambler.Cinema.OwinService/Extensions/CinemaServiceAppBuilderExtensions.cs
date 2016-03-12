using System;
using System.Web.Http;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Infrastructure;
using Owin;
using Rambler.Cinema.OwinService.Configuration;
using Rambler.Cinema.OwinService.Windsor;

namespace Rambler.Cinema.OwinService.Extensions
{
    public static class CinemaServiceAppBuilderExtensions
    {
        public static void UseRamblerCinema (this IAppBuilder app, RamblerCinemaServiceOptions opt)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var configuration = new HttpConfiguration();

            var windsorContainer = WindsorConfig.Configure(opt);
            app.UseWindsorDependencyResolverScope(configuration, windsorContainer);

            //adding conversions from Func to OwinMiddleWare and back
            SignatureConversions.AddConversions(app); 

            RouteConfig.RegisterRoutes(configuration);
            WebApiConfig.Configure(configuration);
            app.UseWebApi(configuration);

            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
