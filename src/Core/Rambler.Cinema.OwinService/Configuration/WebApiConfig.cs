using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using Rambler.WebApiHelpers;

namespace Rambler.Cinema.OwinService.Configuration
{
    static class WebApiConfig
    {
        internal static void Configure (HttpConfiguration config)
        {
            //Web API uses content negotiation. We are going to provide only JSON responses
            config.Formatters.Clear();
            config.Formatters.Add(new JsonDefaultFormatter());
            config.Formatters.Add(new JsonIdentedFormatter());

            //allowing passing Web Api requests without specifying some required members
            //foreach (var formatter in config.Formatters)            
                //formatter.RequiredMemberSelector = new SuppressedRequiredMemberSelector();
            
            config.Services.Replace(typeof(IContentNegotiator),
                new JsonContentNegotiator(excludeMatchOnTypeOnly: true));

            //Adding for each action request DTO validator
            config.Filters.Add(new InvalidModelStateFilterAttribute());

            config.Services.RemoveAll(typeof(ModelValidatorProvider),
                validator => !(validator is DataAnnotationsModelValidatorProvider));

            config.MessageHandlers.Add(new OptionsHandler());
        }
    }
}
