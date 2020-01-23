using System.Data;

namespace NerdyMishka.Data.MetaData
{
    public class BooleanTypeMap : RelationalTypeMap
    {
        public BooleanTypeMap(string sqlType, DbType? dbType)
            : base(sqlType, typeof(bool), dbType)
        {
        }

        public override string WriteLiteral(object instance)
            => (bool)instance ? "1" : "0";
    }
}