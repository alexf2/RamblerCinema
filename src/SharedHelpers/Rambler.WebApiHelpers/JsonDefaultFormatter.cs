using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rambler.WebApiHelpers
{
    public class JsonDefaultFormatter: JsonMediaTypeFormatter
    {
        public JsonDefaultFormatter(): base()
        {
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
        }

        protected JsonDefaultFormatter(JsonMediaTypeFormatter formatter): base(formatter)
        {            
        }
    }
}
