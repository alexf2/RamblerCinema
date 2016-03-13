using System.Collections.Generic;
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
    public class OptionsHandler: DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var res1 = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (request.Method == HttpMethod.Options)
            {
                return await Task.Run(() =>
                {
                    var methods = new ActionSelector(request).GetSupportedMethods();
                    
                    var res = new HttpResponseMessage(HttpStatusCode.OK);
                    res.Content = new StringContent(string.Empty);
                    res.Content.Headers.Add("Allow", methods);
                    res.Content.Headers.Add("Allow", "OPTIONS");

                    return res;
                }).ConfigureAwait(false);
            }
            else
                return res1;
        }

        private class ActionSelector
        {
            readonly HttpRequestMessage _request;
            readonly HttpControllerContext _context;
            readonly ApiControllerActionSelector _apiSelector;
            static readonly string[] Methods = { "GET", "PUT", "POST", "PATCH", "DELETE", "HEAD", "TRACE" };

            public ActionSelector(HttpRequestMessage request)
            {
                try
                {
                    
                    var configuration = request.GetConfiguration();
                    var requestContext = request.GetRequestContext();
                        
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

            public IEnumerable<string> GetSupportedMethods()
            {
                return _request == null ? new string[] {} : Methods.Where(IsMethodSupported);
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
