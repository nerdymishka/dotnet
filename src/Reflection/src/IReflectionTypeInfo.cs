using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionTypeInfo : IReflectionMember, IReflect
    {
        bool HasElementType { get; }

        bool IsArray { get; }

        IReflectionTypeInfo BaseType { get; }

        string FullName { get; }

        IReadOnlyCollection<IReflectionConstructor> Constructors { get; }

        IReadOnlyCollection<IReflectionTypeInfo> Interfaces { get; }

        IReflectionTypeInfo LoadInterfaces(bool inherit = false);

        IReflectionTypeInfo LoadFields(bool inherit = false);

        IReflectionTypeInfo LoadProperties(bool inherit = false);

        IReflectionTypeInfo LoadMethods(bool inherit = false);
    }
}