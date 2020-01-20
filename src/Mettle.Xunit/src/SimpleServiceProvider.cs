using System;
using System.Collections.Concurrent;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mettle
{
    public class SimpleServiceProvider : IServiceProvider
    {
        private ConcurrentDictionary<Type, Func<IServiceProvider, object>> factories =
            new ConcurrentDictionary<Type, Func<IServiceProvider, object>>();

        public SimpleServiceProvider()
        {
            this.factories.TryAdd(typeof(IAssert), (s) => { return AssertImpl.Current; });
            this.factories.TryAdd(typeof(ITestOutputHelper), (s) => { return new TestOutputHelper(); });
        }

        public void AddSingleton(Type type, object instance)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.factories.TryAdd(type, (s) => instance);
        }

        public void AddTransient(Type type)
        {
            this.factories.TryAdd(type, (s) => Activator.CreateInstance(type));
        }

        public void AddTransient(Type type, Func<IServiceProvider, object> activator)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (activator == null)
                throw new ArgumentNullException(nameof(activator));

            this.factories.TryAdd(type, activator);
        }

        public object GetService(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (this.factories.TryGetValue(type, out Func<IServiceProvider, object> factory))
                return factory(this);

            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}