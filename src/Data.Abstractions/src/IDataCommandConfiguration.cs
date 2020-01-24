
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NerdyMishka.Data
{
    public interface IDataCommandConfiguration
    {
        ParameterSetType SetType { get; }

        IEnumerable<IDbDataParameter> DbParameters { get; }

        IEnumerable<KeyValuePair<string, object>> Parameters { get; }

        char ParameterPrefix { get; set; }

        IDictionary Hastable { get; }

        IList ParameterArray { get; }

        void SetParameters(IList value);

        void SetParameters(IDictionary value);

        void SetParameters(IEnumerable<IDbDataParameter> value);

        void SetParameters(IEnumerable<KeyValuePair<string, object>> value);

        StringBuilder Query { get; }

        CommandBehavior CommandBehavior { get; set; }

        CommandType CommandType { get; set; }

        bool IsCompleteable { get; set; }
    }
}