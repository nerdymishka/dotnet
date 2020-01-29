using System;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionFactory : IReflectionFactory
    {
        public virtual IReflectionParameter CreateParameter(ParameterInfo info)
        {
            return new ReflectionParameter(info);
        }

        public virtual IReflectionMethod CreateMethod(
            MethodInfo info,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null)
        {
            return new ReflectionMethod(
                info,
                this,
                parameters,
                declaringType);
        }

        public virtual IReflectionProperty CreateProperty(
            PropertyInfo info,
            IReflectionTypeInfo declaringType = null)
        {
            return new ReflectionProperty(info, declaringType);
        }

        public virtual IReflectionField CreateField(
            FieldInfo info,
            IReflectionTypeInfo declaringType = null)
        {
            return new ReflectionField(info, declaringType);
        }

        public virtual IReflectionTypeInfo CreateType(Type info)
        {
            return null;
        }

        public virtual IReflectionTypeInfo CreateType(TypeInfo info)
        {
            return null;
        }

        public virtual IReflectionInterface CreateInterface(Type info)
        {
            return null;
        }

        public virtual IReflectionInterface CreateInterface(TypeInfo info)
        {
            return null;
        }

        public virtual IReflectionConstructor CreateConstructor(
            ConstructorInfo info,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null)
        {
            return new ReflectionConstructor(
                info,
                this,
                parameters,
                declaringType);
        }
    }
}