using System;
using System.Data;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for objects that hold SQL type information. e.g. parameters
    /// columns.
    /// </summary>
    public interface ISqlTypeInfo
    {
        /// <summary>
        /// Gets the size. Generally for columns such as VARCHAR.
        /// </summary>
        /// <value>The size.</value>
        int? Size { get; }

        /// <summary>
        /// Gets the precision. Generally for decimal values.
        /// </summary>
        /// <value>
        /// The precision.
        /// </value>
        int? Precision { get; }

        /// <summary>
        /// Gets the scale. Generally for decimal values.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        int? Scale { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        bool IsNullable { get; }

        /// <summary>
        /// Gets the type of the color.
        /// </summary>
        /// <value>
        /// The type of the color.
        /// </value>
        Type ClrType { get; }

        /// <summary>
        /// Gets the normalized SQL database type.
        /// </summary>
        /// <value>
        /// The type of the database.
        /// </value>
        DbType DbType { get; }

        /// <summary>
        /// Gets the raw SQL type such as VARCHAR.
        /// </summary>
        /// <value>
        /// The raw SQL type.
        /// </value>
        string SqlType { get; }
    }
}