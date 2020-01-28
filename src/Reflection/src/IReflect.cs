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

        IReflectionField GetField(
            string name,
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance);

        IEnumerable<IReflectionField> GetDeclaredFields();

        IReflectionProperty GetProperty(
            string name,
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance);

        IEnumerable<IReflectionProperty> GetDeclaredProperties();

        IReflectionMethod GetMethod(
            string name,
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance,
            Type[] genericArgTypes = null,
            Type[] parameterTypes = null);

        IEnumerable<IReflectionMethod> GetDeclaredMethods();
    }
}