using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionEnum : IReflectionTypeInfo
    {
        IReadOnlyCollection<string> EnumNames { get; }

        IReadOnlyCollection<object> EnumValues { get; }

        string GetEnumName(object value);
    }
}