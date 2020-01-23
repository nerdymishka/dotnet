
namespace NerdyMishka.Data.MetaData
{
    public class BooleanTypeMap : RelationalTypeMap
    {
        public BooleanTypeMap(
            string sqlType,
            dbType? type) :
            base(sqlType, typeof(bool), type)
        {

        }

        public override WriteLiteral(object instance)
            => (bool)instance ? "1" : "0";
    }
}