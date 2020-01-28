using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public abstract class ReflectionMember : IReflectionMember
    {
        private List<Attribute> attributes;

        private Dictionary<string, bool> flags;

        private Dictionary<string, object> metadata;

        public abstract string Name { get; }

        public abstract Type ClrType { get; }

        public virtual IReadOnlyCollection<Attribute> Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.LoadAttributes();
                }

                return this.attributes;
            }
        }

        public Dictionary<string, bool> Flags
        {
            get
            {
                this.flags = this.flags ?? new Dictionary<string, bool>();
                return this.flags;
            }
        }

        public Dictionary<string, object> Metadata
        {
            get
            {
                this.metadata = this.metadata ?? new Dictionary<string, object>();
                return this.metadata;
            }
        }

        public T GetAnnotation<T>(string name)
        {
            if (this.Metadata.TryGetValue(name, out object value))
                return (T)value;

            return default(T);
        }

        public IReflectionMember SetAnnotation<T>(string name, T value)
        {
            this.Metadata[name] = value;
            return this;
        }

        public bool HasFlag(string flag)
        {
            if (this.Flags.TryGetValue(flag, out bool result))
                return result;

            return false;
        }

        public IReflectionMember SetFlag(string flag, bool value)
        {
            this.Flags[flag] = value;
            return this;
        }

        public virtual IReflectionMember LoadAttributes(bool inherit = true)
        {
            this.SetAttributes(
                CustomAttributeExtensions.GetCustomAttributes(this.ClrType, inherit));

            return this;
        }

        public virtual TAttribute FindAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)this.Attributes
                .FirstOrDefault(o => o is TAttribute);
        }

        public virtual IReadOnlyCollection<TAttribute> FindAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return this.Attributes.Where(o => o is TAttribute)
                .Cast<TAttribute>()
                .ToList();
        }

        protected void SetAttributes(IEnumerable<Attribute> range)
        {
            this.attributes = new List<Attribute>(range);
        }
    }
}