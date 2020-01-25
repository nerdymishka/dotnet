using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
#pragma warning disable CA1010

namespace Fakes
{
    public class FakeDbParameterCollection : DbParameterCollection,
        IDataParameterCollection
    {
        private readonly object syncRoot = new object();
        private List<DbParameter> parameters =
            new List<DbParameter>();

        public override int Count => this.parameters.Count;

        public override object SyncRoot => this.syncRoot;

        public override int Add(object value)
        {
            if (value is DbParameter parameter)
                this.parameters.Add(parameter);

            return this.Count;
        }

        public override void AddRange(Array values)
        {
            foreach (var item in values)
            {
                if (item is DbParameter parameter)
                    this.parameters.Add(parameter);
            }
        }

        public override void Clear()
        {
            this.parameters.Clear();
        }

        public override bool Contains(object value)
        {
            if (value is DbParameter parameter)
                this.parameters.Contains(parameter);

            return false;
        }

        public override bool Contains(string value)
        {
            return this.parameters.Any(o => o.ParameterName == value);
        }

        public override void CopyTo(Array array, int index)
        {
            var set = new DbParameter[this.parameters.Count];
            this.parameters.CopyTo(set, index);
            Array.Copy(set, index, array, 0, array.Length);
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (var item in this.parameters)
                yield return item;
        }

        public override int IndexOf(object value)
            => this.parameters.IndexOf((DbParameter)value);

        public override int IndexOf(string parameterName)
            => this.parameters.FindIndex(0, o => o.ParameterName == parameterName);

        public override void Insert(int index, object value)
            => this.parameters.Insert(index, (DbParameter)value);

        public override void Remove(object value)
            => this.parameters.Remove((DbParameter)value);

        public override void RemoveAt(int index)
            => this.parameters.RemoveAt(index);

        public override void RemoveAt(string parameterName)
            => this.parameters.Remove(this.GetParameter(parameterName));

        protected override DbParameter GetParameter(int index)
            => this.parameters[index];

        protected override DbParameter GetParameter(string parameterName)
            => this.parameters.SingleOrDefault(o => o.ParameterName == parameterName);

        protected override void SetParameter(int index, DbParameter value)
            => this.parameters[index] = value;

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var idx = this.IndexOf(parameterName);
            this.parameters[idx] = value;
        }
    }
}