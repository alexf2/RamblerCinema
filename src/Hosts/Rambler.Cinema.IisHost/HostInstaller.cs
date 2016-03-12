using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rambler.Cinema.Core.Contract;
using Rambler.Cinema.DataProviders;

namespace Rambler.Cinema.IisHost
{
    public sealed class HostInstaller: IWindsorInstaller
    {
        public void Install (IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHelloProvider>().ImplementedBy<HelloProvider>()
            );
        }
    }
}