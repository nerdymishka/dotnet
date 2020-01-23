using System;
using System.Data;

namespace NerdyMishka.Data
{
    public interface ISqlTypeInfo
    {
        int? Size { get; }

        int? Precision { get; }

        int? Scale { get; }

        bool IsNullable { get; }

        Type ClrType { get; }

        DbType DbType { get; }

        string SqlType { get; }
    }
}