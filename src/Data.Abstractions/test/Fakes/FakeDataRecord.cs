using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace Fakes
{
    public class FakeDataRecord : DbDataRecord
    {
        private int fieldCount = -1;
        private object record;
        private List<object> fields;
        private List<PropertyInfo> properties;

        protected internal FakeDataRecord(
            object record,
            List<PropertyInfo> properties)
        {
            this.properties = properties;
            this.record = record;
            this.fields = new List<object>();

            foreach (var prop in this.properties)
            {
                this.fields.Add(prop.GetValue(this.record));
            }
        }

        public override int FieldCount => this.properties.Count;

        public override object this[int ordinal]
            => this.GetValue(ordinal);

        public override object this[string name]
            => this.GetValue(this.GetOrdinal(name));

        public override bool GetBoolean(int ordinal)
            => (bool)this.GetValue(ordinal);

        public override byte GetByte(int ordinal)
            => (byte)this.GetValue(ordinal);

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            var chars = (byte[])this.GetValue(ordinal);
            if (dataOffset >= chars.Length)
                throw new ArgumentOutOfRangeException(nameof(dataOffset));

            var ceiling = (chars.LongLength - 1) - dataOffset;
            long l = length;
            if (length > ceiling)
                l = ceiling;

            Array.Copy(chars, dataOffset, buffer, bufferOffset, l);
            return l;
        }

        public override char GetChar(int ordinal)
            => (char)this.GetValue(ordinal);

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            var chars = (char[])this.GetValue(ordinal);
            if (dataOffset >= chars.Length)
                throw new ArgumentOutOfRangeException(nameof(dataOffset));

            var ceiling = (chars.LongLength - 1) - dataOffset;
            long l = length;
            if (length > ceiling)
                l = ceiling;

            Array.Copy(chars, dataOffset, buffer, bufferOffset, l);
            return l;
        }

        public override string GetDataTypeName(int ordinal)
            => this.properties[ordinal].PropertyType.Name;

        public override DateTime GetDateTime(int ordinal)
             => (DateTime)this.GetValue(ordinal);

        public override decimal GetDecimal(int ordinal)
             => Convert.ToDecimal(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override double GetDouble(int ordinal)
            => Convert.ToDouble(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override Type GetFieldType(int ordinal)
            => this.properties[ordinal].PropertyType;

        public override float GetFloat(int ordinal)
             => Convert.ToSingle(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override Guid GetGuid(int ordinal)
            => (Guid)this.GetValue(ordinal);

        public override short GetInt16(int ordinal)
            => Convert.ToInt16(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override int GetInt32(int ordinal)
            => Convert.ToInt32(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override long GetInt64(int ordinal)
            => Convert.ToInt64(this.GetValue(ordinal), CultureInfo.InvariantCulture);

        public override string GetName(int ordinal)
            => this.properties[ordinal].Name;

        public override int GetOrdinal(string name)
        {
            int i = 0;
            foreach (var item in this.properties)
            {
                if (item.Name == name)
                    return i;
            }

            return -1;
        }

        public override string GetString(int ordinal)
            => this.GetValue(ordinal).ToString();

        public override object GetValue(int ordinal)
            => this.fields[ordinal];

        public override int GetValues(object[] values)
        {
            this.fields.CopyTo(values);
            return this.fields.Count;
        }

        public override bool IsDBNull(int ordinal)
            => this.fields[ordinal] == null;
    }
}