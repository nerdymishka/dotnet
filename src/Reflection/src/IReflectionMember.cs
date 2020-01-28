using System;
using System.Collections.Generic;

namespace NerdyMishka.Reflection
{
    /// <summary>
    /// The core contract for all reflection metadata.
    /// </summary>
    public interface IReflectionMember
    {
        IReadOnlyCollection<Attribute> Attributes { get; }

        string Name { get; }

        Type ClrType { get; }

        bool HasFlag(string flag);

        IReflectionMember SetFlag(string flag, bool value);

        T GetMetadata<T>(string name);

        IReflectionMember SetMetadata<T>(string name, T value);

        IReflectionMember LoadAttributes(bool inherit = true);

        T FindAttribute<T>()
            where T : Attribute;

        IEnumerable<T> FindAttributes<T>()
            where T : Attribute;
    }
}