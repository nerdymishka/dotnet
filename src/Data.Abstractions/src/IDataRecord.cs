using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Provides access to the column values within each row for a <c>IDataReader</c>.
    /// </summary>
    public interface IDataRecord
    {
        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>The number of columns in the current row.</value>
        int FieldCount { get; }

        /// <summary>
        /// Gets the value for the item at the given index.
        /// </summary>
        /// <value>The value for the item at the given index.</value>
        /// <param name="i">The column index.</param>
        object this[int i] { get; }

        /// <summary>
        /// Gets the value for the item with the given name.
        /// </summary>
        /// <value>The value for the item with the given name.</value>
        /// <param name="name">The column name.</param>
        object this[string name] { get; }

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />; Otherwise, <c>false</c>.</returns>
        bool IsDbNull(int ordinal);

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.</returns>
        Task<bool> IsDbNullAsync(int ordinal);

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <param name="cancellationToken">The token to cancel the task.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />; Otherwise, <c>false</c>.</returns>
        Task<bool> IsDbNullAsync(int ordinal, CancellationToken cancellationToken);

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.</returns>
        bool IsDbNull(string name);

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.</returns>
        Task<bool> IsDbNullAsync(string name);

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">The token to cancel the task.</param>
        /// <returns><c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.</returns>
        Task<bool> IsDbNullAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="bool"/> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>The <see cref="bool" /> value.</returns>
        bool GetBoolean(int ordinal);

        /// <summary>
        /// Gets a <see cref="bool"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="bool" /> value.</returns>
        bool GetBoolean(string name);

        /// <summary>
        /// Gets a <see cref="byte" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>The <see cref="byte" /> value.</returns>
        byte GetByte(int ordinal);

        /// <summary>
        /// Gets a <see cref="byte" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="byte" /> value.</returns>
        byte GetByte(string name);

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int offset, int length);

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>The number of bytes read.</returns>
        long GetBytes(int ordinal, byte[] buffer);

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        long GetBytes(string name, long fieldOffset, byte[] buffer, int offset, int length);

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>The number of bytes read.</returns>
        long GetBytes(string name, byte[] buffer);

        /// <summary>
        /// Gets a <see cref="System.Char"> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>The <see cref="System.Char" /> value.</returns>
        char GetChar(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Char"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Char" /> value.</returns>
        char GetChar(string name);

        /// <summary>
        /// Gets the <see cref="System.Char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        long GetChars(int ordinal, long fieldOffset, char[] buffer, int offset, int length);

        /// <summary>
        /// Gets the <see cref="System.Char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>The number of bytes read.</returns>
        long GetChars(int ordinal, char[] buffer);

        /// <summary>
        /// Gets the <see cref="System.Char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        long GetChars(string name, long fieldOffset, char[] buffer, int offset, int length);

        /// <summary>
        /// Gets the <see cref="System.Char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>The number of bytes read.</returns>
        long GetChars(string name, char[] buffer);

        /// <summary>
        /// Gets the date type name for the given column.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>The name of the data type.</returns>
        string GetDataTypeName(int ordinal);

        /// <summary>
        /// Gets the data type name for the given column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The name of the data type.</returns>
        string GetDataTypeName(string name);

        /// <summary>
        /// Gets a <see cref="System.DateTime"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.DateTime" /> value.</returns>
        DateTime GetDateTime(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.DateTime"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.DateTime" /> value.</returns>
        DateTime GetDateTime(string name);

        /// <summary>
        /// Gets a <see cref="System.Decimal" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Decimal" /> value.</returns>
        decimal GetDecimal(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Decimal"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Decimal" /> value.</returns>
        decimal GetDecimal(string name);

        /// <summary>
        /// Gets a <see cref="System.Double" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Double" /> value.</returns>
        double GetDouble(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Double"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Double" /> value.</returns>
        double GetDouble(string name);

        /// <summary>
        /// Gets the <see cref="System.Type"/> by column name for the value.
        /// </summary>
        /// <param name="name">The column index.</param>
        /// <returns>The <see cref="System.Type" />.</returns>
        Type GetFieldType(int ordinal);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type GetFieldType(string name);

        /// <summary>
        /// Gets a <see cref="System.Single" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Single" /> value.</returns>
        float GetFloat(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Single"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Single" /> value.</returns>
        float GetFloat(string name);

        /// <summary>
        /// Gets a <see cref="System.Guid" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Guid" /> value.</returns>
        Guid GetGuid(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Guid"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Guid" /> value.</returns>
        Guid GetGuid(string name);

        /// <summary>
        /// Gets a <see cref="System.Int16" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Int16" /> value.</returns>
        short GetInt16(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Int16"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Int16" /> value.</returns>
        short GetInt16(string name);

        /// <summary>
        /// Gets a <see cref="System.Int32" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Int32" /> value.</returns>
        int GetInt32(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Int32"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Int32" /> value.</returns>
        int GetInt32(string name);

        /// <summary>
        /// Gets a <see cref="System.Int64" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Int64" /> value.</returns>
        long GetInt64(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Int64"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.Int64" /> value.</returns>
        long GetInt64(string name);

        /// <summary>
        /// Gets the name of the column at the given index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>The name of the column.</returns>
        string GetName(int ordinal);

        /// <summary>
        /// Gets the index of the column for the given name.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The index.</returns>
        int GetOrdinal(string name);

        /// <summary>
        /// Gets a <see cref="System.String" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.String" /> value.</returns>
        string GetString(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.String"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.String" /> value.</returns>
        string GetString(string name);

        /// <summary>
        /// Gets a <typeparamref name="T"/> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <typeparam name="T">The data type.</typeparam>
        /// <returns>The column value.</returns>
        T GetValueAs<T>(int ordinal);

        /// <summary>
        /// Gets a <typeparamref name="T"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <typeparam name="T">The data type.</typeparam>
        /// <returns>The column value.</returns>
        T GetValueAs<T>(string name);

        /// <summary>
        /// Gets a <see cref="System.Object" /> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.Object" /> value.</returns>
        object GetValue(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.Object" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The column value.</returns>
        object GetValue(string name);

        /// <summary>
        /// Gets all the values for the current row.
        /// </summary>
        /// <param name="values">The values buffer.</param>
        /// <returns>The number of values written.</returns>
        int GetValues(object[] values);

        /// <summary>
        /// Gets a <see cref="System.IO.Stream"/> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.IO.Stream" /> value.</returns>
        Stream GetStream(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.IO.Stream"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.IO.Stream" /> value.</returns>
        Stream GetStream(string name);

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader"/> value by column index.
        /// </summary>
        /// <param name="index">The column index.</param>
        /// <returns>The <see cref="System.IO.TextReader" /> value.</returns>
        TextReader GetTextReader(int ordinal);

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader"/> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The <see cref="System.IO.TextReader" /> value.</returns>
        TextReader GetTextReader(string name);
    }
}