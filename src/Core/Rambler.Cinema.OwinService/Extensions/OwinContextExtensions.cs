using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Owin;
using Owin;
using Rambler.Cinema.OwinService.Configuration;

namespace Rambler.Cinema.OwinService.Extensions
{
    public static class OwinContextExtensions
    {
        const string DEPENDENCY_RESOLVER_KEY = "IDependencyResolver";

        public static IAppBuilder UseDependencyResolver (this IAppBuilder app, HttpConfiguration config, IDependencyResolver container)
        {            
            config.DependencyResolver = container;
            return app.Use<IoCContainerMiddleware>(container);
        }

        public static void SetDependencyResolver (this IOwinContext ctx, IDependencyResolver resolver)
        {
            ctx.Environment[ DEPENDENCY_RESOLVER_KEY ] = resolver;
        }

        public static IDependencyResolver GetDependencyResolver (this IOwinContext ctx)
        {
            return ctx.Environment[ DEPENDENCY_RESOLVER_KEY ] as IDependencyResolver;
        }

        public static TService GetService<TService> (this IDependencyResolver resolver) where TService : class
        {
            return resolver.GetService(typeof(TService)) as TService;
        }
    }
}
