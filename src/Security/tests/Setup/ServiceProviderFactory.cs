using System;
using Mettle;

namespace Tests
{
    public class ServiceProviderFactory : IServiceProviderFactory
    {
        public IServiceProvider CreateProvider()
        {
            var provider = new Mettle.SimpleServiceProvider();

            return provider;
        }
    }
}