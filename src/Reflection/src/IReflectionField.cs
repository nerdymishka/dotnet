using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionField : IReflectionMember,
        IReflectionValueAccessor
    {
        FieldInfo FieldInfo { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        IReflectionModifierAccess ModifierAccess { get; }
    }
}