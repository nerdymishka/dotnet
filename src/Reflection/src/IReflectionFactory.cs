using System;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionFactory
    {
        IReflectionParameter CreateParameter(ParameterInfo info,
            IReflectionTypeInfo declaringType = null);

        IReflectionMethod CreateMethod(MethodInfo info,
            IReflectionTypeInfo declaringType = null);

        IReflectionMethod CreateMethod(
            MethodInfo info,
            ParameterInfo[] parameters,
            IReflectionTypeInfo declaringType = null);

        IReflectionProperty CreateProperty(PropertyInfo info, IReflectionTypeInfo declaringType = null);

        IReflectionProperty CreateProperty(FieldInfo info, IReflectionTypeInfo declaringType = null);

        IReflectionTypeInfo CreateType(Type info);

        IReflectionTypeInfo CreateType(TypeInfo info);

        IReflectionInterface CreateInterface(Type info);

        IReflectionInterface CreateInterface(TypeInfo info);

        IReflectionConstructor CreateConstructor(
            ConstructorInfo info,
            IReflectionTypeInfo declaringType = null);
    }
}