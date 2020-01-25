using System;
using System.Collections.Generic;
using System.Text;

namespace NerdyMishka.Data.MetaData
{
    public class NullTypeMap : RelationalTypeMap
    {
        public NullTypeMap(string storeType)
            : base(storeType, typeof(object))
        {
        }
    }
}
