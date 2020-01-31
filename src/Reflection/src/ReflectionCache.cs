using System;
using System.Collections.Concurrent;

namespace NerdyMishka.Reflection
{
    public class ReflectionCache : IReflectionCache
    {
        private ReflectionFactory factory;

        private ConcurrentDictionary<Type, IReflectionTypeInfo> cache =
            new ConcurrentDictionary<Type, IReflectionTypeInfo>();

        public ReflectionCache(int capacity = -1, int concurrencyLevel = 100)
        {
            if (capacity < 0)
                this.cache = new ConcurrentDictionary<Type, IReflectionTypeInfo>();
            else
                this.cache = new ConcurrentDictionary<Type, IReflectionTypeInfo>(concurrencyLevel, capacity);
        }

        public static ReflectionCache GlobalCache { get; set; } = new ReflectionCache();

        public virtual IReflectionFactory ReflectionFactory
        {
            get
            {
                this.factory = this.factory ?? new ReflectionFactory(this);
                return this.factory;
            }
        }

        public virtual void Clear()
        {
            this.cache.Clear();
        }

        public virtual bool TryRemove(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return this.cache.TryRemove(type, out IReflectionTypeInfo result);
        }

        public virtual bool TryRemove(IReflectionTypeInfo type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return this.cache.TryRemove(
                type.ClrType,
                out type);
        }

        public virtual IReflectionTypeInfo GetOrAdd(Type type)
        {
            if (this.cache.TryGetValue(type, out IReflectionTypeInfo reflectedType))
                return reflectedType;

            reflectedType = this.ReflectionFactory.CreateType(type);
            this.cache.TryAdd(type, reflectedType);
            return reflectedType;
        }
    }
}