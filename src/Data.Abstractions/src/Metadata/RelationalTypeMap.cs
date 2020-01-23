using System;
using System.Data;

namespace NerdyMishka.Data.MetaData
{
    public class RelationalTypeMap : ISqlTypeMap
    {

        public class NullTypeMap : RelationalTypeMap
        {

            public NullTypeMap(string storeType)
                : base(storeType, typeof(object))
            {

            }
        }

        public readonly static RelationalTypeMap Null = new NullTypeMap("NULL");

        public RelationalTypeMap(
            string sqlType,
            Type clrType,
            DbType? dbType = null,
            int? size = null,
            bool unicode = false
        )
        {
            this.SqlType = sqlType;
            this.ClrType = clrType;
            this.DbType = dbType;
        }

        public RelationalTypeMap(
            string storeType,
            Type clrType,
            DbType? dbType = null,
            int? precision = null,
            int? scale = null)
            : this(storeType, clrType, dbType, null, false)
        {

        }

        public virtual int? Size { get; protected set; }

        public virtual int? Precision { get; protected set; }

        public virtual bool? Scale { get; protected set; }

        public virtual bool IsNullable { get; protected set; }

        public virtual Type ClrType { get; protected set; }

        public virtual DbType DbType { get; protected set; }

        public virtual string SqlType { get; protected set; }

        public virtual string FormatProvider => "{0}";


        public virtual void ConfigureParameter(IDbDataParameter parameter)
        {
        }

        public virtual string WriteLiteral(object instance)
            => string.Format(this.FormatProvider, instance);
    }
}