using System;

namespace NerdyMishka.Reflection
{
    public interface IReflectionListTypeInfo
    {
        IReflectionTypeInfo GetElementType();
    }
}