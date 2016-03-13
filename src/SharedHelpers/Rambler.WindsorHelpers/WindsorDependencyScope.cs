using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace Rambler.WindsorHelpers
{
    internal sealed class WindsorDependencyScope: IDependencyScope
    {
        readonly IWindsorContainer _container;
        IDisposable _scope;

        public WindsorDependencyScope(IWindsorContainer container)
        {
            _container = container;
            _scope = container.RequireScope();
        }

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            var scope = Interlocked.Exchange(ref _scope, null);
            scope?.Dispose();
        }
    }
}
