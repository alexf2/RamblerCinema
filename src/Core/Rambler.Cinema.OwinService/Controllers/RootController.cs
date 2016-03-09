using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Rambler.Shared.Contract.ResponseDTO;

namespace Rambler.Cinema.OwinService.Controllers
{
    public class RootController: ApiController
    {
        [ResponseType(typeof(ServiceInfo))]
        public IHttpActionResult Get()
        {
            return Ok(new ServiceInfo()
            {
                ApplicationName = "Rambler.Cinema service",
                ApiVersion = GetProductVersion(),
                Links = new {}
            });
        }

        public static string GetProductVersion()
        {
            var attribute = (AssemblyInformationalVersionAttribute)Assembly
              .GetExecutingAssembly()
              .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true)
              .Single();

            return attribute.InformationalVersion;
        }
    }
}
