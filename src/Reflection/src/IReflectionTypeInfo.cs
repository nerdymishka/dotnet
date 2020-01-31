using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionTypeInfo : IReflectionMember, IReflect
    {
        bool HasElementType { get; }

        bool IsArray { get; }

        bool IsListLike { get; }

        bool IsDictionaryLike { get; }

        bool IsNullableOfT { get; }

        bool IsAbstract { get; }

        bool IsByRef { get; }

        bool IsClass { get; }

        bool IsEnum { get; }

        bool IsInterface { get; }

        bool IsValueType { get; }

        bool IsPrimitive { get; }

        bool IsPublic { get; }

        bool IsNotPublic { get; }

        bool IsGenericType { get; }

        bool IsGenericTypeDefinition { get; }

        bool IsSealed { get; }

        bool IsDataType { get; }

        IReflectionTypeInfo UnderlyingType { get; }

        IReflectionTypeInfo BaseType { get; }

        string FullName { get; }

        IReadOnlyCollection<IReflectionConstructor> Constructors { get; }

        IReadOnlyCollection<IReflectionTypeInfo> Interfaces { get; }

        IReflectionTypeInfo GetElementType();

        IReflectionTypeInfo GetKeyType();

        IReflectionTypeInfo LoadInterfaces();

        IReflectionTypeInfo LoadFields(
            bool includeStatic = false,
            bool includeInherit = false);

        IReflectionTypeInfo LoadFields(BindingFlags flags);

        IReflectionTypeInfo LoadProperties(
            bool includeStatic = false,
            bool includeInherit = false);

        IReflectionTypeInfo LoadProperties(BindingFlags flags);

        IReflectionTypeInfo LoadMethods(
            bool includeStatic = false,
            bool includeInherit = false);

        IReflectionTypeInfo LoadMethods(BindingFlags flags);
    }
}