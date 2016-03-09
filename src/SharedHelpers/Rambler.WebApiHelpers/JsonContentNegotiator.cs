using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Rambler.WebApiHelpers
{
    public class JsonContentNegotiator: DefaultContentNegotiator
    {
        public JsonContentNegotiator ()
            : this(false)
        {
        }
        
        public JsonContentNegotiator (bool excludeMatchOnTypeOnly): base(excludeMatchOnTypeOnly)
        {            
        }

        public override ContentNegotiationResult Negotiate (Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var res = base.Negotiate(type, request, formatters);

            if (res.Formatter is JsonDefaultFormatter && request.GetQueryStrings().ContainsKey("idented"))
            {
                var fmtIdented = formatters.FirstOrDefault(f => f.GetType() == typeof (JsonIdentedFormatter));
                if (fmtIdented != null)
                    res.Formatter = fmtIdented;
            }

            return res;
        }
    }
}
