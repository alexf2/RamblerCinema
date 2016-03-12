using System.Linq;
using System.Reflection;

namespace Rambler.WebApiHelpers
{
    public static class VersionHelpers
    {
        public static string GetProductVersion(Assembly ass)
        {
            var attribute = (AssemblyInformationalVersionAttribute) (ass ?? Assembly.GetExecutingAssembly())
              .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true)
              .Single();

            return attribute.InformationalVersion;
        }
    }
}
