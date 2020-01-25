using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public interface IDataConnectionActions : IDisposable
    {
        string Provider { get; }

        string ConnectionString { get; set; }

        ConnectionState State { get; }

        void Open();

        Task OpenAsync(CancellationToken cancellationToken = default);

        void Close();
    }
}