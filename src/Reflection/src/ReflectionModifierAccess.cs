namespace NerdyMishka.Reflection
{
    public abstract class ReflectionModifierAccess : IReflectionModifierAccess
    {
        public abstract bool IsStatic { get; }

        public abstract bool IsPublic { get; }

        public abstract bool IsPrivate { get; }

        public abstract bool IsInstance { get; }

        public abstract bool IsVirtual { get; }

        public abstract bool IsProtected { get; }

        public abstract bool IsInternal { get; }
    }
}