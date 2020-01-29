using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionMethod : IReflectionMethodBase
    {
        MethodInfo MethodInfo { get; }

        IReflectionParameter ReturnParameter { get; }

        object Invoke(object instance, params object[] parameters);
    }
}