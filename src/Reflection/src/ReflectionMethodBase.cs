using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionMethodBase : ReflectionMember,
        IReflectionMethodBase,
        IReflectionFactorySource
    {
        private List<IReflectionParameter> parameters;

        private Type[] generateTypeArguments;

        private Type[] parameterTypes;

        private IReflectionModifierAccess modifierAccess;

        public ReflectionMethodBase(
            MethodBase methodBase,
            IReflectionFactory factory,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null)
        {
            if (methodBase == null)
                throw new ArgumentNullException(nameof(methodBase));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            this.MethodBase = methodBase;
            this.ReflectionFactory = factory;
            this.DeclaringType = declaringType;

            if (parameters != null)
            {
                this.parameters = new List<IReflectionParameter>();

                foreach (var param in parameters)
                {
                    this.parameters.Add(this.ReflectionFactory.CreateParameter(param));
                }
            }
        }

        public override string Name => this.MethodBase.Name;

        public override Type ClrType => this.MethodBase.DeclaringType;

        public IReflectionTypeInfo DeclaringType { get; private set; }

        public MethodBase MethodBase { get; protected set; }

        public bool IsGenericMethodDefinition =>
            this.MethodBase.IsGenericMethodDefinition;

        public IReflectionModifierAccess ModifierAccess
        {
            get
            {
                this.modifierAccess = this.modifierAccess ??
                    new MethodModifierAccess(this.MethodBase);

                return this.modifierAccess;
            }
        }

        public IReadOnlyCollection<Type> GenericArguments
        {
            get
            {
                this.generateTypeArguments = this.generateTypeArguments ??
                    this.MethodBase.GetGenericArguments();

                return this.generateTypeArguments;
            }
        }

        public virtual IReadOnlyCollection<IReflectionParameter> Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new List<IReflectionParameter>();
                    foreach (var param in this.MethodBase.GetParameters())
                    {
                        this.parameters.Add(this.ReflectionFactory.CreateParameter(param));
                    }
                }

                return this.parameters;
            }
        }

        public virtual IReadOnlyCollection<Type> ParameterTypes
        {
            get
            {
                this.parameterTypes = this.parameterTypes ?? this.Parameters
                    .Select(o => o.ClrType)
                    .ToArray();

                return this.parameterTypes;
            }
        }

        public IReflectionFactory ReflectionFactory { get; private set; }
    }
}