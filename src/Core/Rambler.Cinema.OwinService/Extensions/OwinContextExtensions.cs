using System.Web.Http;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Microsoft.Owin;
using Owin;
using Rambler.Cinema.OwinService.Configuration;
using Rambler.Cinema.OwinService.Windsor;

namespace Rambler.Cinema.OwinService.Extensions
{
    public static class OwinContextExtensions
    {
        const string DEPENDENCY_RESOLVER_KEY = "IDependencyResolver";

        public static IAppBuilder UseWindsorDependencyResolverScope (this IAppBuilder app, HttpConfiguration config, IWindsorContainer container)
        {
            var windsorResolver = new WindsorDependencyResolver(container);
            config.DependencyResolver = windsorResolver;
            return app.Use<IoCContainerMiddleware>(windsorResolver);
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
