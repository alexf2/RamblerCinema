using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Rambler.WebApiHelpers
{
    public class JsonIdentedFormatter: JsonDefaultFormatter
    {
        public JsonIdentedFormatter(): base()
        {            
            SerializerSettings.Formatting = Formatting.Indented;
        }

        protected JsonIdentedFormatter(JsonMediaTypeFormatter formatter): base(formatter)
        {
        }
    }
}
