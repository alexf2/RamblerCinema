using System.Web.Http;
using System.Web.Http.Description;
using Rambler.Cinema.Core.Contract;

using Rambler.Shared.Contract.ResponseDTO;
using Rambler.WebApiHelpers;

namespace Rambler.Cinema.OwinService.Controllers
{
    public class RootController: ApiController
    {
        readonly IHelloProvider _prov;
        public RootController (IHelloProvider prov)
        {
            _prov = prov;
        }

        [ResponseType(typeof(ServiceInfo))]
        public IHttpActionResult Get()
        {
            return Ok(new ServiceInfo()
            {
                ApplicationName = "Rambler.Cinema service",
                ApiVersion = VersionHelpers.GetProductVersion(GetType().Assembly),
                Links = new {}
            });
        }

        [HttpGet]
        public IHttpActionResult Hello (string name)
        {
            return Ok(_prov.Hello(name));
        }
    }
}
