using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rambler.Shared.Contract.ResponseDTO
{
    public sealed class ServiceInfo
    {
        public string ApplicationName { get; set; }
        public string ApiVersion { get; set; }
        public object Links { get; set; }
    }
}
