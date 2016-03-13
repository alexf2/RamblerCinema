using System;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;

namespace Rambler.Cinema.Core.Contract
{
    public sealed class RamblerCinemaServiceOptions
    {
        public RamblerCinemaServiceOptions (IDependencyResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            Resolver = resolver;
        }
        
        public IDependencyResolver Resolver { get; private set; }

        public IExceptionLogger ExceptionLogger { get; set; }
        public IExceptionHandler ExceptionHandler { get; set; }
    }
}
