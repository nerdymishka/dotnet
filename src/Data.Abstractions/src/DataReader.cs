using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Wraps objects that implement <see cref="System.Data.IDataReader"/>.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataReader" />
    public class DataReader : IDataReader
    {
        private System.Data.IDataReader reader;

        private DbDataReader dbReader;

        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="ArgumentNullException">reader.</exception>
        public DataReader(System.Data.IDataReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.reader = reader;
            if (reader is DbDataReader)
                this.dbReader = (DbDataReader)reader;
        }

        /// <summary>
        /// Gets the depth of nesting for the current row.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public int Depth
            => this.reader.Depth;

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>
        /// The number of columns in the current row.
        /// </value>
        public int FieldCount
            => this.reader.FieldCount;

        /// <summary>
        /// Gets a value indicating whether or not this data set has rows.
        /// </summary>
        /// <value>
        /// The value indicating whether or not this data set has rows.
        /// </value>
        public bool HasRows =>
            this.dbReader?.HasRows ?? true;

        /// <summary>
        /// Gets a value indicating whether or not the data reader is closed.
        /// </summary>
        /// <value>
        /// A value indicating whether or not the data reader is closed.
        /// </value>
        public bool IsClosed =>
            this.reader.IsClosed;

        /// <summary>
        /// Gets a value indicating whether this instance is database data reader.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is database data reader; otherwise, <c>false</c>.
        /// </value>
        public bool IsDbDataReader =>
            this.dbReader != null;

        /// <summary>
        /// Gets the <see cref="object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns>The field value.</returns>
        public object this[string name]
            => this.reader.GetValue(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets the <see cref="object"/> with the specified ordinal.
        /// </summary>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The field value.</returns>
        public object this[int ordinal]
            => this.reader.GetValue(ordinal);

        /// <summary>
        /// Gets a <see cref="bool" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="bool" /> value.
        /// </returns>
        public bool GetBoolean(string name)
                    => this.reader.GetBoolean(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="bool" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="bool" /> value.
        /// </returns>
        public bool GetBoolean(int ordinal)
                    => this.reader.GetBoolean(ordinal);

        /// <summary>
        /// Gets a <see cref="byte" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="byte" /> value.
        /// </returns>
        public byte GetByte(string name)
                    => this.reader.GetByte(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="byte" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="byte" /> value.
        /// </returns>
        public byte GetByte(int ordinal)
                    => this.reader.GetByte(ordinal);

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

            return this.reader.GetBytes(
                this.reader.GetOrdinal(name),
                0,
                buffer,
                0,
                buffer.Length);
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

            return this.reader.GetBytes(
                ordinal,
                0,
                buffer,
                0,
                buffer.Length);
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
        public long GetBytes(
                    string name,
                    long fieldOffset,
                    byte[] buffer,
                    int offset,
                    int length)
        {
            return this.reader.GetBytes(
                this.reader.GetOrdinal(name),
                0,
                buffer,
                offset,
                length);
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
        public long GetBytes(
                    int ordinal,
                    long fieldOffset,
                    byte[] buffer,
                    int offset,
                    int length)
        {
            return this.reader.GetBytes(
                ordinal,
                fieldOffset,
                buffer,
                offset,
                length);
        }

        /// <summary>
        /// Gets a <see cref="char" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="char" /> value.
        /// </returns>
        public char GetChar(string name)
                    => this.reader.GetChar(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The character.
        /// </returns>
        public char GetChar(int ordinal)
                    => this.reader.GetChar(ordinal);

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

            return this.reader.GetChars(
                this.reader.GetOrdinal(name),
                0,
                buffer,
                0,
                buffer.Length);
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
            return this.reader.GetChars(ordinal, 0, buffer, 0, buffer.Length);
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
        public long GetChars(
                    string name,
                    long fieldOffset,
                    char[] buffer,
                    int offset,
                    int length)
        {
            return this.reader.GetChars(
                this.reader.GetOrdinal(name),
                0,
                buffer,
                offset,
                length);
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
        public long GetChars(
                    int ordinal,
                    long fieldOffset,
                    char[] buffer,
                    int offset,
                    int length)
        {
            return this.reader.GetChars(ordinal, 0, buffer, offset, length);
        }

        /// <summary>
        /// Gets the data type name for the given column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The name of the data type.
        /// </returns>
        public string GetDataTypeName(string name)
                    => this.reader.GetDataTypeName(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets the date type name for the given column.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The name of the data type.
        /// </returns>
        public string GetDataTypeName(int ordinal)
                    => this.reader.GetDataTypeName(ordinal);

        /// <summary>
        /// Gets a <see cref="System.DateTime" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.DateTime" /> value.
        /// </returns>
        public DateTime GetDateTime(string name)
                    => this.reader.GetDateTime(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="System.DateTime" /> value by column name.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.DateTime" /> value.
        /// </returns>
        public DateTime GetDateTime(int ordinal)
                    => this.reader.GetDateTime(ordinal);

        /// <summary>
        /// Gets a <see cref="decimal" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="decimal" /> value.
        /// </returns>
        public decimal GetDecimal(string name)
                    => this.reader.GetDecimal(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="decimal" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="decimal" /> value.
        /// </returns>
        public decimal GetDecimal(int ordinal)
                    => this.reader.GetDecimal(ordinal);

        /// <summary>
        /// Gets a <see cref="double" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="double" /> value.
        /// </returns>
        public double GetDouble(string name)
                    => this.reader.GetDouble(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="double" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="double" /> value.
        /// </returns>
        public double GetDouble(int ordinal)
                    => this.reader.GetDouble(ordinal);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IDataRecord> GetEnumerator()
        {
            string[] names = null;
            while (this.Read())
            {
                if (names == null)
                {
                    var list = new List<string>();
                    int i = 0,
                        l = this.FieldCount;

                    while (i < l)
                    {
                        list.Add(this.GetName(i));
                        i++;
                    }

                    names = list.ToArray();
                }

                var values = new object[this.FieldCount];
                this.GetValues(values);

                yield return new DataRecord(names, values);
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Type" /> for the field.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <c>Type</c> of field.
        /// </returns>
        public Type GetFieldType(string name)
                    => this.reader.GetFieldType(
                        this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets the <see cref="System.Type" /> by column name for the value.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.Type" />.
        /// </returns>
        public Type GetFieldType(int ordinal)
                    => this.reader.GetFieldType(ordinal);

        /// <summary>
        /// Gets a <see cref="float" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="float" /> value.
        /// </returns>
        public float GetFloat(string name)
                    => this.reader.GetFloat(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="float" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="float" /> value.
        /// </returns>
        public float GetFloat(int ordinal)
                    => this.reader.GetFloat(ordinal);

        /// <summary>
        /// Gets a <see cref="System.Guid" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.Guid" /> value.
        /// </returns>
        public Guid GetGuid(string name)
                    => this.reader.GetGuid(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="System.Guid" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.Guid" /> value.
        /// </returns>
        public Guid GetGuid(int ordinal)
                    => this.reader.GetGuid(ordinal);

        /// <summary>
        /// Gets a <see cref="short" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="short" /> value.
        /// </returns>
        public short GetInt16(string name)
                    => this.reader.GetInt16(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="short" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="short" /> value.
        /// </returns>
        public short GetInt16(int ordinal)
                    => this.reader.GetInt16(ordinal);

        /// <summary>
        /// Gets a <see cref="int" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="int" /> value.
        /// </returns>
        public int GetInt32(string name)
                    => this.reader.GetInt32(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="int" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="int" /> value.
        /// </returns>
        public int GetInt32(int ordinal)
                    => this.reader.GetInt32(ordinal);

        /// <summary>
        /// Gets a <see cref="long" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="long" /> value.
        /// </returns>
        public long GetInt64(string name)
                    => this.reader.GetInt64(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="long" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="long" /> value.
        /// </returns>
        public long GetInt64(int ordinal)
                    => this.reader.GetInt64(ordinal);

        /// <summary>
        /// Gets the name of the column at the given index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The name of the column.
        /// </returns>
        public string GetName(int ordinal)
                    => this.reader.GetName(ordinal);

        /// <summary>
        /// Gets the index of the column for the given name.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>
        /// The index.
        /// </returns>
        public int GetOrdinal(string name)
                    => this.reader.GetOrdinal(name);

        /// <summary>
        /// Gets a <see cref="System.IO.Stream" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.IO.Stream" /> value.
        /// </returns>
        public Stream GetStream(string name) =>
                    this.dbReader?.GetStream(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="System.IO.Stream" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.IO.Stream" /> value.
        /// </returns>
        public Stream GetStream(int ordinal)
                    => this.dbReader?.GetStream(ordinal);

        /// <summary>
        /// Gets a <see cref="string" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="string" /> value.
        /// </returns>
        public string GetString(string name)
                    => this.reader.GetString(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="string" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="string" /> value.
        /// </returns>
        public string GetString(int ordinal)
                    => this.reader.GetString(ordinal);

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The <see cref="System.IO.TextReader" /> value.
        /// </returns>
        public TextReader GetTextReader(string name)
                    => this.dbReader?.GetTextReader(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="System.IO.TextReader" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="System.IO.TextReader" /> value.
        /// </returns>
        public TextReader GetTextReader(int ordinal)
                    => this.dbReader?.GetTextReader(ordinal);

        /// <summary>
        /// Gets a <see cref="object" /> value by column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public object GetValue(string name)
                    => this.reader.GetValue(this.reader.GetOrdinal(name));

        /// <summary>
        /// Gets a <see cref="object" /> value by column index.
        /// </summary>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The <see cref="object" /> value.
        /// </returns>
        public object GetValue(int ordinal)
                    => this.reader.GetValue(ordinal);

        /// <summary>
        /// Gets a <typeparamref name="T" /> value by column name.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="name">The column name.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public T GetValueAs<T>(string name)
        {
            var value = this.reader.GetValue(this.reader.GetOrdinal(name));
            if (value is DBNull)
                return default;

            return (T)value;
        }

        /// <summary>
        /// Gets a <typeparamref name="T" /> value by column index.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="ordinal">The column index.</param>
        /// <returns>
        /// The column value.
        /// </returns>
        public T GetValueAs<T>(int ordinal)
        {
            var value = this.reader.GetValue(ordinal);
            if (value is DBNull)
                return default;

            return (T)value;
        }

        /// <summary>
        /// Gets all the values for the current row.
        /// </summary>
        /// <param name="values">The values buffer.</param>
        /// <returns>
        /// The number of values written.
        /// </returns>
        public int GetValues(object[] values)
                    => this.reader.GetValues(values);

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
                    => this.reader.IsDBNull(this.GetOrdinal(name));

        /// <summary>
        /// Determines if the value for the given index is the same as
        /// <see cref="DbNull" />.
        /// </summary>
        /// <param name="ordinal">The index of the column.</param>
        /// <returns>
        ///   <c>True</c> if the value is <see cref="DbNull" />; Otherwise, <c>false</c>.
        /// </returns>
        public bool IsDbNull(int ordinal)
                    => this.reader.IsDBNull(ordinal);

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
                    => this.dbReader?.IsDBNullAsync(this.reader.GetOrdinal(name));

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
                    => this.dbReader?.IsDBNullAsync(ordinal);

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
        {
            return this.dbReader?.IsDBNullAsync(this.reader.GetOrdinal(name), cancellationToken);
        }

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
        {
            return this.dbReader?.IsDBNullAsync(ordinal, cancellationToken);
        }

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the read moves to the next result set;
        /// Otherwise, <c>False</c>.
        /// </returns>
        public bool NextResult() => this.reader.NextResult();

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public Task<bool> NextResultAsync()
                    => this.dbReader?.NextResultAsync();

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <param name="cancellationToken">The cancel token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task<bool> NextResultAsync(CancellationToken cancellationToken)
        {
            return this.dbReader?.NextResultAsync(cancellationToken);
        }

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the read moved to the next row;
        /// Otherwise, <c>False</c>.
        /// </returns>
        public bool Read() => this.reader.Read();

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public Task<bool> ReadAsync() =>
                    this.dbReader?.ReadAsync();

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <param name="cancellationToken">The cancel token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public Task<bool> ReadAsync(CancellationToken cancellationToken)
                    => this.dbReader?.ReadAsync(cancellationToken);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Unwraps the current object by returning the inner object.
        /// </summary>
        /// <returns>
        /// The inner object.
        /// </returns>
        object IUnwrap.Unwrap()
        {
            return this.reader;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            if (disposing)
            {
                this.reader.Dispose();
                this.reader = null;
            }

            this.isDisposed = true;
        }
    }
}