using System;
using System.Text;
using System.Reflection;

namespace NerdyMishka.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendTransform(
            this StringBuilder builder,
            string identifier,
            ITextTransform transformer)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (transformer is null)
                throw new ArgumentNullException(nameof(transformer));

            return builder.Append(
                transformer.Transform(identifier));
        }

        public static StringBuilder AppendQuote(
            this StringBuilder builder,
            string value,
            char leftQuote = '"',
            char rightQuote = '"')
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            return builder.Append(leftQuote)
                    .Append(value)
                    .Append(rightQuote);
        }

        public static StringBuilder AppendQuote(
            this StringBuilder builder,
            string value,
            ITextTransform transformer,
            char leftQuote = '"',
            char rightQuote = '"')
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (transformer is null)
                throw new ArgumentNullException(nameof(transformer));

            return builder.Append(leftQuote)
                    .Append(transformer.Transform(value))
                    .Append(rightQuote);
        }
    }
}