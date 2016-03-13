using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rambler.Cinema.Core.Contract;
using Rambler.Cinema.DataProviders;
using Rambler.WebApiHelpers;
using Rambler.WindsorHelpers;
using System.Web.Http.ExceptionHandling;
using Rambler.Shared.Contract;

namespace Rambler.Cinema.IisHost
{
    public sealed class IisHostInstaller: IWindsorInstaller
    {
        public void Install (IWindsorContainer container, IConfigurationStore store)
        {
            BootstrapLogger.Instance.Debug("Starting Iis host installer");

            container.Register(
                Component.For<Microsoft.Owin.Logging.ILoggerFactory, IExtendedLoggerFactory>().ImplementedBy<WindsorLoggerFactoryAdapter>(),
                Component.For<IExceptionLogger>().UsingFactoryMethod( (k) => new Rambler.WindsorHelpers.ExceptionLogger(k.Resolve<IExtendedLoggerFactory>().CreateExt(LoggerNames.UnhandledExc))),

                Component.For<IHelloProvider>().ImplementedBy<HelloProvider>()                
            );

            BootstrapLogger.Instance.Debug("Iis host installer finished");
        }
    }
}