using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionConstructor : IReflectionMethodBase
    {
        ConstructorInfo ConstructorInfo { get; }

        object Invoke(params object[] parameters);
    }
}