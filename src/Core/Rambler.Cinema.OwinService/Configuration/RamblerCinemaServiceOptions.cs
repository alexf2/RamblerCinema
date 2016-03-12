using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;

namespace Rambler.Cinema.OwinService.Configuration
{
    public sealed class RamblerCinemaServiceOptions
    {
        public RamblerCinemaServiceOptions ()
        {
            Installers = new AssemblyInstaller[] {};
        }

        public RamblerCinemaServiceOptions (IWindsorInstaller extra)
        {
            Installers = new[] { extra };
        }

        public RamblerCinemaServiceOptions(params IWindsorInstaller[] extra)
        {
            Installers = (IWindsorInstaller[])extra.Clone();
        }

        public IWindsorInstaller[] Installers { get; private set; }
    }
}
