using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionProperty : ReflectionMember, IReflectionProperty
    {
        private Delegate getter;
        private Delegate setter;

        private IReflectionModifierAccess getterAccess;

        private IReflectionModifierAccess setterAccess;

        public ReflectionProperty(PropertyInfo info, IReflectionTypeInfo delcaringType = null)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            this.PropertyInfo = info;
            this.DeclaringType = delcaringType;
        }

        public override string Name => this.PropertyInfo.Name;

        public override Type ClrType => this.PropertyInfo.PropertyType;

        public bool CanRead => this.PropertyInfo.CanRead;

        public bool CanWrite => this.PropertyInfo.CanWrite;

        public IReflectionModifierAccess GetterAccess
        {
            get
            {
                this.getterAccess = this.getterAccess ?? new MethodModifierAccess(
                    this.PropertyInfo.GetMethod);

                return this.getterAccess;
            }
        }

        public IReflectionModifierAccess SetterAccess
        {
            get
            {
                this.setterAccess = this.setterAccess ?? new MethodModifierAccess(
                    this.PropertyInfo.SetMethod);

                return this.getterAccess;
            }
        }

        public PropertyInfo PropertyInfo { get; protected set; }

        public IReflectionTypeInfo DeclaringType { get; protected set; }

        public virtual object GetValue(object instance)
        {
            if (!this.CanRead)
                throw new InvalidOperationException($"Property {this.Name} prohibits reading the value.");

            if (this.getter == null)
            {
                if (this.PropertyInfo.GetMethod.IsStatic)
                {
                    var invokeGet = Expression.Property(null, this.PropertyInfo);
                    this.getter = Expression
                        .Lambda(Expression.Block(invokeGet))
                        .Compile();
                }
                else
                {
                    var oVariable = Expression.Parameter(this.PropertyInfo.DeclaringType, "o");
                    var invokeGet = Expression.Property(oVariable, this.PropertyInfo);
                    var b = Expression.Block(invokeGet);
                    this.getter = Expression
                        .Lambda(b, oVariable)
                        .Compile();
                }
            }

            if (!this.PropertyInfo.GetMethod.IsStatic)
                return this.getter.DynamicInvoke(instance);

            return this.getter.DynamicInvoke();
        }

        public virtual void SetValue(object instance, object value)
        {
            if (!this.CanWrite)
                throw new InvalidOperationException($"Property {this.Name} prohibits writing the value.");

            if (this.setter == null)
            {
                if (this.PropertyInfo.SetMethod.IsStatic)
                {
                    var invokeSet = Expression.Property(null, this.PropertyInfo);
                    var valueVariable = Expression.Variable(this.ClrType, "value");
                    var b = Expression.Block(Expression.Assign(invokeSet, valueVariable));
                    this.setter = Expression
                        .Lambda(b, valueVariable)
                        .Compile();
                }
                else
                {
                    var oVariable = Expression.Parameter(this.PropertyInfo.DeclaringType, "o");
                    var invokeSet = Expression.Property(oVariable, this.PropertyInfo);
                    var valueVariable = Expression.Variable(this.ClrType, "value");
                    var b = Expression.Block(
                        Expression.Assign(invokeSet, valueVariable));

                    this.setter = Expression
                        .Lambda(b, oVariable, valueVariable)
                        .Compile();
                }
            }

            if (!this.PropertyInfo.SetMethod.IsStatic)
                this.setter.DynamicInvoke(instance, value);
            else
                this.setter.DynamicInvoke(value);
        }

        public override IReflectionMember LoadAttributes(bool inherit = true)
        {
            this.SetAttributes(
                CustomAttributeExtensions.GetCustomAttributes(this.PropertyInfo, inherit));

            return this;
        }

        protected class MethodModifierAccess : ReflectionModifierAccess
        {
            private MethodBase info;

            public MethodModifierAccess(MethodBase info)
            {
                this.info = info;
            }

            public override bool IsStatic => this.info.IsStatic;

            public override bool IsPublic => this.info.IsPublic;

            public override bool IsPrivate => this.info.IsPrivate;

            public override bool IsInstance => !this.info.IsStatic;

            public override bool IsVirtual => false;

            public override bool IsProtected => this.info.IsFamily;

            public override bool IsInternal => this.info.IsAssembly;
        }
    }
}