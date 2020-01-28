using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionField : ReflectionMember, IReflectionField
    {
        private Delegate getter;

        private Delegate setter;

        private IReflectionModifierAccess access;

        public ReflectionField(FieldInfo info, IReflectionTypeInfo delcaringType = null)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            this.FieldInfo = info;
            this.DeclaringType = delcaringType;
        }

        public IReflectionTypeInfo DeclaringType { get; protected set; }

        public FieldInfo FieldInfo { get; protected set; }

        public override string Name => this.FieldInfo.Name;

        public override Type ClrType => this.FieldInfo.FieldType;

        public bool CanRead => true;

        public bool CanWrite => !this.FieldInfo.IsInitOnly;

        public IReflectionModifierAccess ModifierAccess
        {
            get
            {
                this.access = this.access ?? new FieldModifierAccess(this.FieldInfo);
                return this.access;
            }
        }

        public virtual object GetValue(object instance)
        {
            if (this.getter == null)
            {
                MemberExpression invokeGet;
                if (this.FieldInfo.IsStatic)
                {
                    invokeGet = Expression.Field(null, this.FieldInfo);
                    this.getter = Expression
                        .Lambda(Expression.Block(invokeGet))
                        .Compile();
                }
                else
                {
                    var oVariable = Expression.Parameter(this.FieldInfo.DeclaringType, "o");
                    invokeGet = Expression.Field(oVariable, this.FieldInfo);
                    this.getter = Expression
                        .Lambda(Expression.Block(invokeGet), oVariable)
                        .Compile();
                }
            }

            if (!this.FieldInfo.IsStatic)
                return this.getter.DynamicInvoke(instance);

            return this.getter.DynamicInvoke();
        }

        public virtual void SetValue(object instance, object value)
        {
            if (!this.CanWrite)
                throw new InvalidOperationException($"Property {this.Name} prohibits reading the value.");

            if (this.setter == null)
            {
                if (this.FieldInfo.IsStatic)
                {
                    var invokeSet = Expression.Field(null, this.FieldInfo);
                    var valueVariable = Expression.Variable(this.FieldInfo.FieldType, "value");
                    var b = Expression.Block(
                        Expression.Assign(invokeSet, valueVariable));

                    this.setter = Expression.Lambda(b, valueVariable).Compile();
                }
                else
                {
                    var oVariable = Expression.Parameter(this.FieldInfo.DeclaringType, "o");
                    var invokeSet = Expression.Field(oVariable, this.FieldInfo);
                    var valueVariable = Expression.Variable(this.FieldInfo.FieldType, "value");
                    var b = Expression.Block(
                        Expression.Assign(invokeSet, valueVariable));

                    this.setter = Expression.Lambda(b, oVariable, valueVariable).Compile();
                }
            }

            if (!this.FieldInfo.IsStatic)
                this.setter.DynamicInvoke(instance, value);
            else
                this.setter.DynamicInvoke(value);
        }

        public override IReflectionMember LoadAttributes(bool inherit = true)
        {
            this.SetAttributes(
                CustomAttributeExtensions.GetCustomAttributes(this.FieldInfo, inherit));

            return this;
        }

        protected class FieldModifierAccess : ReflectionModifierAccess
        {
            private FieldInfo info;

            public FieldModifierAccess(FieldInfo info)
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