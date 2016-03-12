using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace Rambler.WebApiHelpers
{
    public class NoCachedResponseFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            base.OnActionExecuted(actionContext);

            if (actionContext != null && actionContext.Response != null && actionContext.Response.IsSuccessStatusCode)
            {
                
                actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue() {NoStore = true, NoCache  = true, Private = true, MaxAge = TimeSpan.Zero };                
                actionContext.Response.Headers.Pragma.TryParseAdd("no-cache");
            }            
        }
    }
}
