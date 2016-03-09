using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Infrastructure;
using Owin;
using Rambler.Cinema.OwinService.Configuration;

namespace Rambler.Cinema.OwinService.Extensions
{
    public static class CinemaServiceAppBuilderExtensions
    {
        public static void UseRamblerCinema (this IAppBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            //adding conversions from Func to OwinMiddleWare and back
            SignatureConversions.AddConversions(app); 

            var configuration = new HttpConfiguration();            
            RouteConfig.RegisterRoutes(configuration);
            WebApiConfig.Configure(configuration);
            app.UseWebApi(configuration);

            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
