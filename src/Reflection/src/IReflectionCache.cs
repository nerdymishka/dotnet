using System;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionCache : IReflectionFactorySource, IDisposable
    {
        void Clear();

        bool TryRemove(IReflectionTypeInfo typeInfo);

        IReflectionTypeInfo GetOrAdd(Type type);
    }
}