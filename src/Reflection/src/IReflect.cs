using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflect
    {
        IReadOnlyCollection<IReflectionField> Fields { get; }

        IReadOnlyCollection<IReflectionProperty> Properties { get; }

        IReadOnlyCollection<IReflectionMethod> Methods { get; }
    }
}