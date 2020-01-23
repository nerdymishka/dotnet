using System.Data;

namespace NerdyMishka.Data
{
    public interface ISqlTypeMap : ISqlTypeInfo
    {
        string FormatProvider { get; }

        bool IsUnicode { get; }

        void ConfigureParameter(IDbDataParameter parameter);

        string WriteLiteral(object instance);
    }
}