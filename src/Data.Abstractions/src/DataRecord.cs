using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Represents a data record.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataRecord" />
    public class DataRecord : IDataRecord
    {
        private object[] records;
        private string[] names;

        public DataRecord(string[] names, object[] records)
        {
            this.names = names;
            this.records = records;
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>
        /// The number of columns in the current row.
        /// </value>
        public int FieldCount => this.records.Length;

        /// <summary>
        /// Gets the <see cref="object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns>the value.</returns>
        public object this[string name]
        {
            get
            {
                var index = Array.IndexOf(this.names, name);
                return this.records[index];
            }
        }

        /// <summary>
        /// Gets the <see cref="object"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="i">The index.</param>
        /// <returns>The value.</returns>
        public object this[int i]
        {
            get { return this.records[i]; }
        }

        /// <summary>
        /// Gets a <see cref="bool" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="bool" /> value.
        /// </returns>
        public bool GetBoolean(string name) =>
            (bool)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="bool" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="bool" /> value.
        /// </returns>
        public bool GetBoolean(int ordinal) =>
            (bool)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="byte" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="byte" /> value.
        /// </returns>
        public byte GetByte(string name) =>
            (byte)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="byte" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="byte" /> value.
        /// </returns>
        public byte GetByte(int ordinal) =>
            (byte)this.records[ordinal];

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetBytes(string name, byte[] buffer)
        {
            Check.NotNull(nameof(buffer), buffer);

            return this.GetBytes(Array.IndexOf(this.names, name), 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetBytes(int ordinal, byte[] buffer)
        {
            Check.NotNull(nameof(buffer), buffer);
            return this.GetBytes(ordinal, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetBytes(string name, long fieldOffset, byte[] buffer, int offset, int length)
        {
            var value = this.records[Array.IndexOf(this.names, name)];
            var bytes = (byte[])value;

            Array.Copy(bytes, (int)fieldOffset, buffer, offset, length);
            int l = Math.Min(buffer?.Length ?? 0, length);
            return l - offset;
        }

        /// <summary>
        /// Gets the bytes and reads them into the buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int offset, int length)
        {
            var value = this.records[ordinal];
            var bytes = (byte[])value;

            Array.Copy(bytes, (int)fieldOffset, buffer, offset, length);

            return buffer?.Length ?? 0;
        }

        /// <summary>
        /// Gets a <see cref="char" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="char" /> value.
        /// </returns>
        public char GetChar(string name)
            => (char)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The character.
        /// </returns>
        public char GetChar(int ordinal)
            => (char)this.records[ordinal];

        /// <summary>
        /// Gets the <see cref="char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetChars(string name, char[] buffer)
        {
            Check.NotNull(nameof(buffer), buffer);
            return this.GetChars(name, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the <see cref="char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetChars(int ordinal, char[] buffer)
        {
            Check.NotNull(nameof(buffer), buffer);
            return this.GetChars(ordinal, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the <see cref="char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetChars(string name, long fieldOffset, char[] buffer, int offset, int length)
        {
            Check.NotNull(nameof(buffer), buffer);
            var value = this.records[Array.IndexOf(this.names, name)];
            var chars = (char[])value;

            Array.Copy(chars, (int)fieldOffset, buffer, offset, length);

            return buffer.Length;
        }

        /// <summary>
        /// Gets the <see cref="char[]" /> value and reads it into the
        /// buffer.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <param name="fieldOffset">The starting index to read the data.</param>
        /// <param name="buffer">The buffer to store read data.</param>
        /// <param name="offset">The position to start writing in the buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public long GetChars(int ordinal, long fieldOffset, char[] buffer, int offset, int length)
        {
            Check.NotNull(nameof(buffer), buffer);
            var value = this.records[ordinal];
            var chars = (char[])value;

            Array.Copy(chars, (int)fieldOffset, buffer, offset, length);

            return buffer.Length;
        }

        /// <summary>
        /// Gets the data type name for the given column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The name of the data type.
        /// </returns>
        public string GetDataTypeName(string name)
            => this.records[Array.IndexOf(this.names, name)].GetType().FullName;

        /// <summary>
        /// Gets the date type name for the given column.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The name of the data type.
        /// </returns>
        public string GetDataTypeName(int ordinal)
            => this.records[ordinal].GetType().FullName;

        /// <summary>
        /// Gets a <see cref="System.DateTime" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.DateTime" /> value.
        /// </returns>
        public DateTime GetDateTime(string name)
            => (DateTime)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="System.DateTime" /> value by column name.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.DateTime" /> value.
        /// </returns>
        public DateTime GetDateTime(int ordinal)
            => (DateTime)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="decimal" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="decimal" /> value.
        /// </returns>
        public decimal GetDecimal(string name)
            => (decimal)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="decimal" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="decimal" /> value.
        /// </returns>
        public decimal GetDecimal(int ordinal)
            => (decimal)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="double" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="double" /> value.
        /// </returns>
        public double GetDouble(string name)
            => (double)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="double" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="double" /> value.
        /// </returns>
        public double GetDouble(int ordinal)
            => (double)this.records[ordinal];

        /// <summary>
        /// Gets the <see cref="System.Type" /> for the field.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <c>Type</c> of field.
        /// </returns>
        public Type GetFieldType(string name)
            => this.records[Array.IndexOf(this.names, name)].GetType();

        /// <summary>
        /// Gets the <see cref="System.Type" /> by column name for the value.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.Type" />.
        /// </returns>
        public Type GetFieldType(int ordinal)
            => this.records[ordinal].GetType();

        /// <summary>
        /// Gets a <see cref="float" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="float" /> value.
        /// </returns>
        public float GetFloat(string name)
            => (float)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="float" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="float" /> value.
        /// </returns>
        public float GetFloat(int ordinal)
             => (float)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="System.Guid" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.Guid" /> value.
        /// </returns>
        public Guid GetGuid(string name)
             => (Guid)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="System.Guid" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.Guid" /> value.
        /// </returns>
        public Guid GetGuid(int ordinal)
             => (Guid)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="short" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="short" /> value.
        /// </returns>
        public short GetInt16(string name)
             => (short)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="short" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="short" /> value.
        /// </returns>
        public short GetInt16(int ordinal)
             => (short)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="int" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="int" /> value.
        /// </returns>
        public int GetInt32(string name)
             => (int)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="int" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="int" /> value.
        /// </returns>
        public int GetInt32(int ordinal)
             => (int)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="long" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="long" /> value.
        /// </returns>
        public long GetInt64(string name)
             => (long)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="long" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="long" /> value.
        /// </returns>
        public long GetInt64(int ordinal)
             => (long)this.records[ordinal];

        /// <summary>
        /// Gets the name of the column at the given index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The name of the column.
        /// </returns>
        public string GetName(int ordinal)
        {
            return this.names[ordinal];
        }

        /// <summary>
        /// Gets the index of the column for the given name.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// The index.
        /// </returns>
        public int GetOrdinal(string name)
        {
            return Array.IndexOf(this.names, name);
        }

        /// <summary>
        /// Gets a <see cref="System.IO.Stream" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.IO.Stream" /> value.
        /// </returns>
        public Stream GetStream(string name)
             => (Stream)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="System.IO.Stream" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.IO.Stream" /> value.
        /// </returns>
        public Stream GetStream(int ordinal)
             => (Stream)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="string" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="string" /> value.
        /// </returns>
        public string GetString(string name)
             => (string)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="string" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="string" /> value.
        /// </returns>
        public string GetString(int ordinal)
             => (string)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.IO.TextReader" /> value.
        /// </returns>
        public TextReader GetTextReader(string name)
             => (TextReader)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.IO.TextReader" /> value.
        /// </returns>
        public TextReader GetTextReader(int ordinal)
             => (TextReader)this.records[ordinal];

        /// <summary>
        /// Gets a <see cref="object" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public object GetValue(string name)
            => this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <see cref="object" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="object" /> value.
        /// </returns>
        public object GetValue(int ordinal)
            => this.records[ordinal];

        /// <summary>
        /// Gets a <typeparamref name="T" /> value by column name.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public T GetValueAs<T>(string name)
            => (T)this.records[Array.IndexOf(this.names, name)];

        /// <summary>
        /// Gets a <typeparamref name="T" /> value by column index.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public T GetValueAs<T>(int ordinal)
            => (T)this.records[ordinal];

        /// <summary>
        /// Gets all the values for the current row.
        /// </summary>
        /// <param name="values">The values buffer.</param>
        /// <returns>
        /// The number of values written.
        /// </returns>
        public int GetValues(object[] values)
        {
            Check.NotNull(nameof(values), values);
            Array.Copy(this.records, values, values.Length);
            return values.Length;
        }

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// <c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.
        /// </returns>
        public bool IsDbNull(string name)
        {
            return this.records[Array.IndexOf(this.names, name)] == DBNull.Value;
        }

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <returns>
        ///   <c>True</c> if the value is <see cref="DbNull" />; Otherwise, <c>false</c>.
        /// </returns>
        public bool IsDbNull(int ordinal)
        {
            return this.records[ordinal] == DBNull.Value;
        }

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// <c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> IsDbNullAsync(string name)
            => new Task<bool>(() => this.IsDbNull(name));

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <returns>
        /// <c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> IsDbNullAsync(int ordinal)
            => new Task<bool>(() => this.IsDbNull(ordinal));

        /// <summary>
        /// Determines if the value for the given name is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="cancellationToken">The token to cancel the task.</param>
        /// <returns>
        /// <c>True</c> if the value is <see cref="DbNull" />;
        /// Otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> IsDbNullAsync(string name, CancellationToken cancellationToken)
            => new Task<bool>(() => this.IsDbNull(name), cancellationToken);

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <param name="cancellationToken">The token to cancel the task.</param>
        /// <returns>
        ///   <c>True</c> if the value is <see cref="DbNull" />; Otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> IsDbNullAsync(int ordinal, CancellationToken cancellationToken)
            => new Task<bool>(() => this.IsDbNull(ordinal), cancellationToken);
    }
}