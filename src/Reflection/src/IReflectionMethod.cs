using System;
using System.Collections.Generic;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionMethod : IReflectionMethodBase
    {
        MethodInfo MethodInfo { get; }

        object Invoke(object instance, params object[] parameters);
    }
}