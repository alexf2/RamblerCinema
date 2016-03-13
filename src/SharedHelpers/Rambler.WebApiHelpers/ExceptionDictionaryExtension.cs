using System;
using System.Collections;

namespace Rambler.WebApiHelpers
{
    public static class ExceptionDictionaryExtension
    {
        const string GuidKey = "ExceptionGuid";

        public static string AssignGuid(this IDictionary exceptionData)
        {
            if (exceptionData == null || exceptionData.IsReadOnly)
                return string.Empty;

            if (!exceptionData.Contains(GuidKey))
                exceptionData[GuidKey] = Guid.NewGuid().ToString("D");

            return (string)exceptionData[ GuidKey ];
        }

        public static string GetExceptionGuid(this IDictionary exceptionData)
        {
            if (exceptionData == null || !exceptionData.Contains(GuidKey))
                return string.Empty;
            return (string)exceptionData[GuidKey];
        }
    }
}
