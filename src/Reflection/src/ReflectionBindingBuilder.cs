using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionBindingBuilder
    {
        public BindingFlags Flags { get; set; }

        public ReflectionBindingBuilder AddPublic()
        {
            this.Flags |= BindingFlags.Public;
            return this;
        }

        public ReflectionBindingBuilder AddNonPublic()
        {
            this.Flags |= BindingFlags.NonPublic;
            return this;
        }

        public ReflectionBindingBuilder AddInstance()
        {
            this.Flags |= BindingFlags.Instance;
            return this;
        }

        public ReflectionBindingBuilder AddStatic()
        {
            this.Flags |= BindingFlags.Static;
            return this;
        }

        public ReflectionBindingBuilder AddInherit()
        {
            if (this.Flags.HasFlag(BindingFlags.DeclaredOnly))
                this.Flags ^= BindingFlags.DeclaredOnly;

            return this;
        }

        public ReflectionBindingBuilder AddDeclaredOnly()
        {
            this.Flags |= BindingFlags.DeclaredOnly;
            return this;
        }

        public BindingFlags ToFlags()
        {
            var result = this.Flags;
            this.Flags = BindingFlags.Default;
            return result;
        }
    }
}