using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
#pragma warning disable CA1010
#pragma warning disable CA1710

namespace Fakes
{
    public class FakeDataReader : DbDataReader
    {
        private IList table;
        private int position = -1;
        private Type lastType;

        private bool hasNewSet = false;

        private int fieldCount = -1;

        private FakeDataRecord record;

        private List<PropertyInfo> properties;

        public FakeDataReader(IList table)
        {
            this.table = table;
        }

        public override int Depth => 0;

        public override int FieldCount => this.record.FieldCount;

        public override bool HasRows => this.table.Count > 0;

        public override bool IsClosed => this.position >= this.table.Count;

        public override int RecordsAffected => 0;

        public override object this[int ordinal]
            => this.record[ordinal];

        public override object this[string name]
            => this.record[name];

        public override bool GetBoolean(int ordinal)
            => this.record.GetBoolean(ordinal);

        public override byte GetByte(int ordinal)
            => this.record.GetByte(ordinal);

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
            => this.record.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

        public override char GetChar(int ordinal)
            => this.record.GetChar(ordinal);

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
            => this.record.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

        public override string GetDataTypeName(int ordinal)
            => this.record.GetDataTypeName(ordinal);

        public override DateTime GetDateTime(int ordinal)
             => this.record.GetDateTime(ordinal);

        public override decimal GetDecimal(int ordinal)
             => this.record.GetDecimal(ordinal);

        public override double GetDouble(int ordinal)
            => this.record.GetDouble(ordinal);

        public override IEnumerator GetEnumerator()
        {
            Type lastType = null;
            List<PropertyInfo> props = null;
            foreach (var item in this.table)
            {
                if (lastType == null || item.GetType() != lastType)
                {
                    lastType = item.GetType();
                    props = lastType.GetProperties().ToList();
                }

                yield return new FakeDataRecord(item, props);
            }
        }

        public override Type GetFieldType(int ordinal)
            => this.record.GetFieldType(ordinal);

        public override float GetFloat(int ordinal)
             => this.record.GetFloat(ordinal);

        public override Guid GetGuid(int ordinal)
            => this.record.GetGuid(ordinal);

        public override short GetInt16(int ordinal)
            => this.record.GetInt16(ordinal);

        public override int GetInt32(int ordinal)
            => this.record.GetInt32(ordinal);

        public override long GetInt64(int ordinal)
            => this.record.GetInt64(ordinal);

        public override string GetName(int ordinal)
            => this.record.GetName(ordinal);

        public override int GetOrdinal(string name)
            => this.record.GetOrdinal(name);

        public override string GetString(int ordinal)
            => this.record.GetString(ordinal);

        public override object GetValue(int ordinal)
            => this.record.GetValue(ordinal);

        public override int GetValues(object[] values)
            => this.record.GetValues(values);

        public override bool IsDBNull(int ordinal)
            => this.record.IsDBNull(ordinal);

        public override bool NextResult()
        {
            return this.hasNewSet;
        }

        public override bool Read()
        {
            if (this.position >= this.table.Count - 1)
                return false;

            this.position++;
            var next = this.table[this.position];
            if (this.lastType == null)
            {
                this.lastType = next.GetType();
                this.properties = this.lastType.GetProperties().ToList();
                this.fieldCount = this.properties.Count;
            }
            else
            {
                if (this.lastType != next.GetType())
                {
                    this.lastType = null;
                    this.hasNewSet = true;
                    this.fieldCount = 0;
                    this.position--;
                    return false;
                }
            }

            this.record = new FakeDataRecord(next, this.properties);
            return true;
        }
    }
}