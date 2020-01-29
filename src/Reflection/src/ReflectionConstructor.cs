using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionConstructor : ReflectionMethodBase,
        IReflectionConstructor
    {
        private Delegate ctor;

        public ReflectionConstructor(
            ConstructorInfo constructorInfo,
            IReflectionFactory factory,
            ParameterInfo[] parameters = null,
            IReflectionTypeInfo declaringType = null)
            : base(constructorInfo, factory, parameters, declaringType)
        {
            this.ConstructorInfo = constructorInfo;
        }

        public ConstructorInfo ConstructorInfo { get; private set; }

        public object Invoke(params object[] parameters)
        {
            if (this.ctor != null)
                return this.ctor.DynamicInvoke(parameters);

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

            NewExpression newExpression = null;
            if (argumentExpressions.Count == 0)
                newExpression = Expression.New(this.ConstructorInfo);
            else
                newExpression =
                    Expression.New(this.ConstructorInfo, argumentExpressions.ToArray());

            this.ctor = Expression.Lambda<Func<object[], object>>(
                Expression.Convert(newExpression, typeof(object)),
                argumentsExpression).Compile();

            return this.ctor.DynamicInvoke(parameters);
        }
    }
}