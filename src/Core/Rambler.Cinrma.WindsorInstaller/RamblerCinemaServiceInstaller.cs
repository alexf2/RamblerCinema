using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rambler.Cinema.OwinService.Controllers;

namespace Rambler.Cinema.OwinService.Configuration
{
    public sealed class RamblerCinemaServiceInstaller: IWindsorInstaller
    {
        public void Install (IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<RootController>().BasedOn<ApiController>().LifestyleScoped());
        }
    }
}
