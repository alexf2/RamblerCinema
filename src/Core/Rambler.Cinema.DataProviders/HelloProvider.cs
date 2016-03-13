using System;
using Rambler.Cinema.Core.Contract;

namespace Rambler.Cinema.DataProviders
{
    public class HelloProvider : IHelloProvider
    {
        public string Hello (string name)
        {
            //throw new Exception("Test exc 1", new Exception("Test inner"));
            return $"Hello {name}!";
        }
    }
}
