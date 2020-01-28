using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionProperty : IReflectionMember,
        IReflectionValueAccessor
    {
        PropertyInfo PropertyInfo { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        IReflectionModifierAccess GetterAccess { get; }

        IReflectionModifierAccess SetterAccess { get; }
    }
}