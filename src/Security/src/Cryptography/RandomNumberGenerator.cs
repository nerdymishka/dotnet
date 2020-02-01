using System;
using System.Buffers;
using System.Security.Cryptography;

namespace NerdyMishka.Security.Cryptography
{
    public class RandomNumberGenerator : IDisposable
    {
        private bool isDisposed = false;
        private System.Security.Cryptography.RandomNumberGenerator randomNumberGenerator;

        public RandomNumberGenerator(string rngName)
        {
            Check.NotNullOrWhiteSpace(nameof(rngName), rngName);
            this.randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create(rngName);
        }

        public RandomNumberGenerator()
        {
            this.randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
        }

        public byte[] NextBytes(int count)
        {
            if (count == 0)
                return Array.Empty<byte>();

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            var bytes = new byte[count];
            this.NextBytes(bytes);
            return bytes;
        }

        public ReadOnlySpan<byte> NextSpan(int count)
        {
            if (count == 0)
                return Array.Empty<byte>();

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            var bytes = new byte[count];
            this.randomNumberGenerator.GetBytes(bytes);
            return bytes;
        }

        public ReadOnlyMemory<byte> NextMemory(int count)
        {
            if (count == 0)
                return Array.Empty<byte>();

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            var bytes = new byte[count];
            this.randomNumberGenerator.GetBytes(bytes);
            return bytes;
        }

        public void NextBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new NullReferenceException("The parameter 'bytes' must not be null");

            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            this.randomNumberGenerator.GetBytes(bytes);
        }

        public void NextBytes(Span<byte> bytes)
        {
            if (bytes == null)
                throw new NullReferenceException("The parameter 'bytes' must not be null");

            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            var rental = ArrayPool<byte>.Shared.Rent(bytes.Length);
            try
            {
                this.randomNumberGenerator.GetBytes(rental);
                rental.CopyTo(bytes);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rental);
            }
        }

        public void NextBytes(Memory<byte> bytes)
        {
            if (this.isDisposed)
                throw new ObjectDisposedException(nameof(RandomNumberGenerator));

            if (bytes.Length == 0)
                return;

            var rental = ArrayPool<byte>.Shared.Rent(bytes.Length);
            try
            {
                this.randomNumberGenerator.GetBytes(rental);
                rental.CopyTo(bytes);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rental);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.randomNumberGenerator.Dispose();
                    this.randomNumberGenerator = null;
                }

                this.isDisposed = true;
            }
        }
    }
}