using System;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Rambler.Cinema.OwinService.Controllers;

namespace Rambler.Cinema.OwinService.Configuration
{
    static class WindsorConfig
    {
        public static IWindsorContainer Configure (RamblerCinemaServiceOptions config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            container.Kernel.AddFacility<TypedFactoryFacility>();

            var installers = new List<IWindsorInstaller> {FromAssembly.Containing<RootController>()};
            installers.AddRange(config.Installers);

            container.Install(installers.ToArray());

            return container;
        }
    }
}
