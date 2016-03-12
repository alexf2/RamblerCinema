using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rambler.Cinema.OwinService.Configuration
{
    static class RouteConfig
    {
        internal static void RegisterRoutes (HttpConfiguration config)
        {
            var routes = config.Routes;

            //gathering new routing rules, declared in attributes on controllers and actions
            config.MapHttpAttributeRoutes();

            routes.MapHttpRoute(
                "ApiRootRoute",
                "",
                defaults: new { controller = "Root", action = "Get" });

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "{controller}/{key}",
                defaults: new { controller = "RootController", key = RouteParameter.Optional });

            routes.MapHttpRoute(
                "RpcRoute",
                "{controller}/{action}/{name}",
                defaults: new { name = RouteParameter.Optional });
        }
    }
}
