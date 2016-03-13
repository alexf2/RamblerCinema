using System.Threading.Tasks;
using Microsoft.Owin;
using System.Web.Http.Dependencies;
using Rambler.Cinema.OwinService.Extensions;

namespace Rambler.Cinema.OwinService.Configuration
{
    public sealed class IoCContainerMiddleware : OwinMiddleware
    {
        readonly IDependencyResolver _resolver;

        public IoCContainerMiddleware (OwinMiddleware next, IDependencyResolver resolver) : base(next)
        {
            _resolver = resolver;
        }

        public override async Task Invoke (IOwinContext context)
        {
            context.SetDependencyResolver(_resolver);
            using (var scope = _resolver.BeginScope())
            {
                await Next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
