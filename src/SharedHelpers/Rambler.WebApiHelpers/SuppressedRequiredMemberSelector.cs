using System.Net.Http.Formatting;
using System.Reflection;

namespace Rambler.WebApiHelpers
{
    public class SuppressedRequiredMemberSelector
        : IRequiredMemberSelector
    {
        public bool IsRequiredMember(MemberInfo member)
        {
            return false;
        }
    }
}
