using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Rambler.WebApiHelpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InvalidModelStateFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting (HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
                return;
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
        }
    }
}
