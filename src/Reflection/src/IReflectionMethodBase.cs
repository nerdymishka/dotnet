using System;
using System.Collections.Generic;

namespace NerdyMishka.Reflection
{
    public interface IReflectionMethodBase : IReflectionMember
    {
        IReflectionModifierAccess ModifierAccess { get; }

        IReadOnlyCollection<IReflectionParameter> Parameters { get; }

        IReadOnlyCollection<Type> GenericArguments { get; }

        IReadOnlyCollection<Type> ParameterTypes { get; }
    }
}