using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionParameter : IReflectionMember
    {
        ParameterInfo ParameterInfo { get; }

        object DefaultValue { get; }

        int Position { get; }

        bool IsOut { get; }

        bool IsOptional { get; }
    }
}