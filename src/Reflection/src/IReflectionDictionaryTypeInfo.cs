using System;

namespace NerdyMishka.Reflection
{
    public interface IReflectionDictionaryTypeInfo : IReflectionListTypeInfo
    {
        IReflectionTypeInfo GetKeyType();
    }
}