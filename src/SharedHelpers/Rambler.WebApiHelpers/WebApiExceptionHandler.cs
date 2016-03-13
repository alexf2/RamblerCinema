using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Rambler.WebApiHelpers
{
    public sealed class WebApiExceptionHandler: ExceptionHandler
    {
        private class ResultHelper : IHttpActionResult
        {
            readonly Action<HttpResponseMessage> _fillResponseAction;
            readonly HttpStatusCode _code;

            public ResultHelper (HttpStatusCode code, Action<HttpResponseMessage> fillResponseAction)
            {
                _code = code;
                _fillResponseAction = fillResponseAction;
            }            

            public Task<HttpResponseMessage> ExecuteAsync (CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(_code);
                _fillResponseAction(response);
                                
                return Task.FromResult(response);
            }
        }

        private sealed class ErrMsg
        {
            public string Message;
            public string ExceptionMessage;
        }

        public override void Handle (ExceptionHandlerContext context)
        {
            var message = "An error occurred.";
            var exceptionGuid = context.Exception.Data.GetExceptionGuid();
            if (!string.IsNullOrEmpty(exceptionGuid))
                message += $" Look into server Log for the exception details with Guid: {exceptionGuid}";

            var negotiator = context.RequestContext.Configuration.Services.GetContentNegotiator();

            var result = negotiator.Negotiate(typeof(ErrMsg), context.ExceptionContext.Request, context.RequestContext.Configuration.Formatters);

            var fmt = result == null
                ? context.RequestContext.Configuration.Formatters[ 0 ]
                : result.Formatter;

            context.Result = new ResultHelper(
                context.Exception is System.Security.SecurityException ? HttpStatusCode.Unauthorized : HttpStatusCode.InternalServerError,
                (rsp) => 
                {
                    rsp.RequestMessage = context.ExceptionContext.Request;
                    rsp.Content = new ObjectContent<ErrMsg>(new ErrMsg() {Message = message, ExceptionMessage = context.Exception.Message}, fmt);
                    //rsp.Content = new StringContent(message);
                }
            );            
        }
    }
}
