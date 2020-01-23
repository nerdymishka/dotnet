

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public class DataConnection : IDataConnection
    {
        public string Provider => throw new NotImplementedException();

        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConnectionState State => throw new NotImplementedException();

        public ISqlDialect SqlDialect => throw new NotImplementedException();

        protected internal IDbConnection InnerConnection { get; set; }

        public IDataTransactionActions BeginTransaction(IsolationLevel level = 0)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IDataCommand CreateCommand(CommandBehavior behavior = CommandBehavior.Default)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IDataCommandBuilder value)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync()
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataConnection()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}