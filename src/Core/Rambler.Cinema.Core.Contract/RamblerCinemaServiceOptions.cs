using System;
using System.Web.Http.Dependencies;

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
    }
}
