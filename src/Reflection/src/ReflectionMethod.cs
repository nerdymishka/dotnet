using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionMethod : ReflectionMethodBase,
        IReflectionMethod
    {
        private Delegate method;

        private IReflectionParameter returnParameter;

        public ReflectionMethod(
            MethodInfo info,
            IReflectionFactory factory,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null)
            : base(info, factory, parameters, declaringType)
        {
            this.MethodInfo = info;
        }

        public MethodInfo MethodInfo { get; protected set; }

        public virtual IReflectionParameter ReturnParameter
        {
            get
            {
                this.returnParameter = this.returnParameter ??
                    this.ReflectionFactory.CreateParameter(
                        this.MethodInfo.ReturnParameter);

                return this.returnParameter;
            }
        }

        public object Invoke(object instance, params object[] parameters)
        {
            // based upon code from stack overflow.
            // https://stackoverflow.com/questions/10313979/methodinfo-invoke-performance-issue
            if (this.method != null)
                return this.method.DynamicInvoke(instance, parameters);

            var instanceExpression = Expression.Parameter(typeof(object), "obj");
            var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
            var argumentExpressions = new List<Expression>();

            foreach (var parameter in this.Parameters)
            {
                var parameterInfo = parameter.ParameterInfo;

                argumentExpressions.Add(
                    Expression.Convert(
                        Expression.ArrayIndex(argumentsExpression,
                        Expression.Constant(parameter.Position)),
                        parameterInfo.ParameterType));
            }

            UnaryExpression unary = null;
            if (!this.MethodInfo.IsStatic)
            {
                unary = Expression.Convert(instanceExpression, this.MethodInfo.ReflectedType);
            }

            var callExpression = Expression.Call(
                unary, this.MethodInfo, argumentExpressions);

            if (callExpression.Type == typeof(void))
            {
                var voidDelegate = Expression.Lambda<Action<object, object[]>>(
                    callExpression, instanceExpression, argumentsExpression)
                    .Compile();

                Func<object, object[], object> action = (obj, arguments) =>
                {
                    voidDelegate(obj, arguments);
                    return null;
                };

                this.method = action;
            }
            else
            {
                this.method = Expression.Lambda<Func<object, object[], object>>(
                    Expression.Convert(
                        callExpression,
                        typeof(object)),
                    instanceExpression,
                    argumentsExpression)
                    .Compile();
            }

            return this.method.DynamicInvoke(instance, parameters);
        }

        public override IReflectionMember LoadAttributes(bool inherit = true)
        {
            this.SetAttributes(
                CustomAttributeExtensions.GetCustomAttributes(this.MethodInfo, inherit));

            return this;
        }
    }
}