using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Rambler.WebApiHelpers;
using Swashbuckle.Application;

namespace Rambler.Cinema.OwinService.Configuration
{
    static class SwaggerConfig
    {        
        public static void Register (HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion(VersionHelpers.GetProductVersion(Assembly.GetExecutingAssembly()), "Rambler Cinema API")
                    .Description("REST api serving cinema film sessions")
                    .Contact(cc => cc.Name("Rambler Team"));
                c.RootUrl(req => req.RequestUri.GetLeftPart(UriPartial.Authority) + req.GetRequestContext().VirtualPathRoot.TrimEnd('/'));
                c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\{Assembly.GetExecutingAssembly().GetName().Name}.XML");
                c.UseFullTypeNameInSchemaIds();
                
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            })
            .EnableSwaggerUi(c => c.DisableValidator());
        }
    }
}
