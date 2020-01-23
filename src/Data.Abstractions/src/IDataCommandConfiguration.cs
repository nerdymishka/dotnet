
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace NerdyMishka.Data
{
    public interface IDataCommandConfiguration
    {
        ParameterSetType SetType { get; set; }

        IEnumerable<IDbDataParameter> DbParameters { get; set; }

        IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }

        char ParameterPrefix { get; set; }

        IDictionary Hastable { get; }

        IList ParameterArray { get; }

        string Query { get; set; }

        CommandBehavior CommandBehavior { get; set; }

        CommandType CommandType { get; set; }

        bool IsCompleteable { get; set; }
    }
}