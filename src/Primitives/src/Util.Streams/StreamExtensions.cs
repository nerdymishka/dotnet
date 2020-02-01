using System;
using System.Buffers;
using System.IO;

namespace NerdyMishka.Util.Streams
{
    public static class StreamExtensions
    {

#if NETSTANDARD2_0
        public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
        {
            Check.NotNull(nameof(stream), stream);

            byte[] sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                buffer.CopyTo(sharedBuffer);
                stream.Write(sharedBuffer, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

        public static void Write(this BinaryWriter writer, ReadOnlySpan<byte> buffer)
        {
            Check.NotNull(nameof(writer), writer);

            byte[] sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                buffer.CopyTo(sharedBuffer);
                writer.Write(sharedBuffer, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

#endif
    }
}