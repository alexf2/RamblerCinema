using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rambler.Cinema.Core.Contract;
using Rambler.Cinema.DataProviders;
using Rambler.WebApiHelpers;
using Rambler.WindsorHelpers;

namespace Rambler.Cinema.IisHost
{
    public sealed class IisHostInstaller: IWindsorInstaller
    {
        public void Install (IWindsorContainer container, IConfigurationStore store)
        {
            BootstrapLogger.Instance.Debug("Starting Iis host installer");

            container.Register(
                Component.For<Microsoft.Owin.Logging.ILoggerFactory, IExtendedLoggerFactory>().ImplementedBy<WindsorLoggerFactoryAdapter>(),                

                Component.For<IHelloProvider>().ImplementedBy<HelloProvider>().LifestyleScoped()             
            );

            BootstrapLogger.Instance.Debug("Iis host installer finished");
        }
    }
}