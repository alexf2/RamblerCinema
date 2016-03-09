using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Rambler.WebApiHelpers
{
    public class OptionsHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            return await base.SendAsync(request, cancellationToken).ContinueWith(
                task =>
                {
                    var response = task.Result;
                    if (request.Method == HttpMethod.Options)
                    {
                        var methods = new ActionSelector(request).GetSupportedMethods();
                        if (!string.IsNullOrEmpty(methods))
                        {
                            var res = new HttpResponseMessage(HttpStatusCode.OK);
                            res.Headers.Add("Allow", methods + ",OPTIONS");
                            return res;
                        }
                    }
                    return response;
                }, cancellationToken);
        }

        private class ActionSelector
        {
            private readonly HttpRequestMessage _request;
            private readonly HttpControllerContext _context;
            private readonly ApiControllerActionSelector _apiSelector;
            private static readonly string[] Methods = { "GET", "PUT", "POST", "PATCH", "DELETE", "HEAD", "TRACE" };

            public ActionSelector(HttpRequestMessage request)
            {
                try
                {
                    var configuration = request.Properties["MS_HttpConfiguration"] as HttpConfiguration;
                    var requestContext = request.Properties["MS_RequestContext"] as HttpRequestContext;
                    var controllerDescriptor = new DefaultHttpControllerSelector(configuration).SelectController(request);

                    _context = new HttpControllerContext
                    {
                        Request = request,
                        RequestContext = requestContext,
                        Configuration = configuration,
                        ControllerDescriptor = controllerDescriptor
                    };
                }
                catch
                {
                    return;
                }

                _request = _context.Request;
                _apiSelector = new ApiControllerActionSelector();
            }

            public string GetSupportedMethods()
            {
                return _request == null ? null : string.Join(",", Methods.Where(IsMethodSupported));
            }

            private bool IsMethodSupported(string method)
            {
                _context.Request = new HttpRequestMessage(new HttpMethod(method), _request.RequestUri);
                var routeData = _context.RouteData;

                try
                {
                    return _apiSelector.SelectAction(_context) != null;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    _context.RouteData = routeData;
                }
            }
        }
    }
}
