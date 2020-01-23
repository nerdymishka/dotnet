
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

        StringBuilder Query { get; set; }

        CommandBehavior CommandBehavior { get; set; }

        CommandType CommandType { get; set; }

        bool IsCompleteable { get; set; }
    }
}