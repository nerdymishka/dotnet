using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NerdyMishka.Data
{
    internal class DbParametersReadOnlyCollection : IReadOnlyCollection<IDbDataParameter>
    {
        private IDataParameterCollection collection;

        public DbParametersReadOnlyCollection(IDataParameterCollection collection)
        {
            this.collection = collection;
        }

        public int Count => this.collection.Count;

        public void Add(IDbDataParameter parameter)
        {
            this.collection.Add(parameter);
        }

        public IEnumerator<IDbDataParameter> GetEnumerator()
        {
            foreach (IDbDataParameter p in this.collection)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (IDbDataParameter p in this.collection)
                yield return p;
        }
    }
}