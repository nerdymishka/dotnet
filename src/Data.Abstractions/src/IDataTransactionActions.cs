using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public interface IDataTransactionActions : IDisposable
    {
        void SetAutoCommit(bool autoCommit = true);

        void Commit();

        void Rollback();
    }
}