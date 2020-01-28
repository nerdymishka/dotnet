using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionConstructor : IReflectionMethod
    {
        ConstructorInfo ConstructorInfo { get; }

        object Invoke(params object[] parameters);
    }
}