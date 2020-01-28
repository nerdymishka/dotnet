using System;
using System.Data;
using System.Globalization;

namespace NerdyMishka.Data.MetaData
{
    public class RelationalTypeMap : ISqlTypeMap
    {
        public static readonly RelationalTypeMap Null = new NullTypeMap("NULL");

        public RelationalTypeMap(
            string storeType,
            Type clrType,
            DbType? dbType = null,
            bool unicode = false,
            int? size = null,
            int? precision = null,
            int? scale = null)
        {
            this.SqlType = storeType;
            this.ClrType = clrType;
            if (dbType.HasValue)
                this.DbType = dbType.Value;

            this.Scale = scale;
            this.Precision = precision;
            this.Size = size;
            this.IsUnicode = unicode;
        }

        public virtual int? Size { get; protected set; }

        public virtual int? Precision { get; protected set; }

        public virtual int? Scale { get; protected set; }

        public virtual bool IsNullable { get; protected set; }

        public virtual bool IsUnicode { get; protected set; }

        public virtual Type ClrType { get; protected set; }

        public virtual DbType DbType { get; protected set; }

        public virtual string SqlType { get; protected set; }

        public virtual string FormatProvider => "{0}";

        public virtual void ConfigureParameter(IDbDataParameter parameter)
        {
        }

        public virtual string WriteLiteral(object instance)
            => string.Format(CultureInfo.InvariantCulture, this.FormatProvider, instance);
    }
}