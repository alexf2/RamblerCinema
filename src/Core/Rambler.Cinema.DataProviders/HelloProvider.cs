using Rambler.Cinema.Core.Contract;

namespace Rambler.Cinema.DataProviders
{
    public class HelloProvider : IHelloProvider
    {
        public string Hello (string name)
        {
            return $"Hello {name}!";
        }
    }
}
