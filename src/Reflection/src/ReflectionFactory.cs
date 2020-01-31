using System;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionFactory : IReflectionFactory
    {
        internal static readonly IReflectionFactory Default = new ReflectionFactory();

        private IReflectionCache cache = null;

        public ReflectionFactory(ReflectionCache cache = null)
        {
            this.cache = cache;
        }

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

        public virtual IReflectionTypeInfo CreateType(Type type)
        {
            return new ReflectionTypeInfo(type, this.cache);
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