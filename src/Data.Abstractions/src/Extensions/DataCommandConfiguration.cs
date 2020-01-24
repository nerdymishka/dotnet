

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NerdyMishka.Data.Extensions
{

#pragma warning disable CA2227
    public class DataCommandConfiguration : IDataCommandConfiguration
    {
        public ParameterSetType SetType { get; set; }
        public IEnumerable<IDbDataParameter> DbParameters { get; internal set; }
        public IEnumerable<KeyValuePair<string, object>> Parameters { get; internal set; }
        public char ParameterPrefix { get; set; }

        public IDictionary Hastable { get; internal set; }

        public IList ParameterArray { get; internal set; }
        public StringBuilder Query { get; private set; } = new StringBuilder();
        public CommandBehavior CommandBehavior { get; set; }
        public CommandType CommandType { get; set; }
        public bool IsCompleteable { get; set; }

        public void SetParameters(IList value)
        {
            this.ParameterArray = value;
            this.SetType = ParameterSetType.Array;
        }

        public void SetParameters(IDictionary value)
        {
            this.Hastable = value;
            this.SetType = ParameterSetType.Hashtable;
        }

        public void SetParameters(IEnumerable<IDbDataParameter> value)
        {
            this.DbParameters = value;
            this.SetType = ParameterSetType.DbParameters;
        }

        public void SetParameters(IEnumerable<KeyValuePair<string, object>> value)
        {
            this.Parameters = value;
            this.SetType = ParameterSetType.KeyValue;
        }
    }
}