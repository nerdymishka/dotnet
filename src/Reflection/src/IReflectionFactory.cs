using System;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionFactory
    {
        IReflectionParameter CreateParameter(ParameterInfo info);

        IReflectionMethod CreateMethod(
            MethodInfo info,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null);

        IReflectionProperty CreateProperty(PropertyInfo info, IReflectionTypeInfo declaringType = null);

        IReflectionField CreateField(FieldInfo info, IReflectionTypeInfo declaringType = null);

        IReflectionTypeInfo CreateType(Type info);

        IReflectionTypeInfo CreateType(TypeInfo info);

        IReflectionInterface CreateInterface(Type info);

        IReflectionInterface CreateInterface(TypeInfo info);

        IReflectionConstructor CreateConstructor(
            ConstructorInfo info,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null);
    }
}