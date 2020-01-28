using System.Reflection;

namespace NerdyMishka.Reflection
{
    public interface IReflectionField : IReflectionMember,
        IReflectionValueAccessor
    {
        FieldInfo PropertyInfo { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        IReflectionModifierAccess ModifierAccess { get; }
    }
}