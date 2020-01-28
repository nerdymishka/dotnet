namespace NerdyMishka.Reflection
{
    public interface IReflectionModifierAccess
    {
        bool IsStatic { get; }

        bool IsPublic { get; }

        bool IsPrivate { get; }

        bool IsInstance { get; }

        bool IsVirtual { get; }

        bool IsProtected { get; }

        bool IsInternal { get; }
    }
}