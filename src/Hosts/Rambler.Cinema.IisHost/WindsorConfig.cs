using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Rambler.Cinema.OwinService.Configuration;
using Rambler.WindsorHelpers;

namespace Rambler.Cinema.IisHost
{
    static class WindsorConfig
    {
        public static IWindsorContainer Configure ()
        {
            BootstrapLogger.Instance.Debug("Windsor container is being created...");

            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            container.Kernel.AddFacility<TypedFactoryFacility>();
            container.Kernel.AddFacility<LoggingFacility>(m => m.UseNLog().WithConfig("NLog.config"));

            container.Install(FromAssembly.Containing<IisHostInstaller>(), FromAssembly.Containing<RamblerCinemaServiceInstaller>());

            BootstrapLogger.Instance.Debug("Windsor container was created");

            return container;
        }
    }
}
