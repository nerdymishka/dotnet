using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NerdyMishka.Data.Extensions
{
    #pragma warning disable CA2227
    public class DataCommandConfiguration : IDataCommandConfiguration
    {
        /// <summary>
        /// Gets or sets the type of the set.
        /// </summary>
        /// <value>
        /// The type of the set.
        /// </value>
        public ParameterSetType SetType { get; set; }

        public IEnumerable<IDbDataParameter> TypedParameterList { get; internal set; }

        public IEnumerable<KeyValuePair<string, object>> TypedParameterLookup { get; internal set; }

        public char ParameterPrefix { get; set; }

        public IDictionary ParameterLookup { get; internal set; }

        public IList ParameterList { get; internal set; }

        public ISqlBuilder Query { get; set; }

        public CommandBehavior CommandBehavior { get; set; }

        public CommandType CommandType { get; set; }

        public bool IsCompleteable { get; set; }

        public void SetParameters(IList value)
        {
            this.ParameterList = value;
            this.SetType = ParameterSetType.List;
        }

        public void SetParameters(IDictionary value)
        {
            this.ParameterLookup = value;
            this.SetType = ParameterSetType.Lookup;
        }

        public void SetParameters(IEnumerable<IDbDataParameter> value)
        {
            this.TypedParameterList = value;
            this.SetType = ParameterSetType.TypedList;
        }

        public void SetParameters(IEnumerable<KeyValuePair<string, object>> value)
        {
            this.TypedParameterLookup = value;
            this.SetType = ParameterSetType.TypedLookup;
        }
    }
}