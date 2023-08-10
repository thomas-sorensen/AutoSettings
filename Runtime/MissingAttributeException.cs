using System;
using System.Diagnostics.CodeAnalysis;

namespace ShadyPixel.AutoSettings
{
    /// <summary>
    /// Exception for missing attributes on a class, method, or property.
    /// </summary>
    public class MissingAttributeException : Exception
    {
        /// <summary>
        /// Initializes with the type of the missing attribute.
        /// </summary>
        /// <param name="attributeType">Type of missing attribute.</param>
        public MissingAttributeException([NotNull] Type attributeType)
            : base(FormatMessage(attributeType))
        {
        }

        /// <summary>
        /// Initializes with the type of the missing attribute and an inner exception.
        /// </summary>
        /// <param name="attributeType">Type of missing attribute.</param>
        /// <param name="inner">Inner exception cause.</param>
        public MissingAttributeException([NotNull] Type attributeType, Exception inner)
            : base(FormatMessage(attributeType), inner)
        {
        }

        /// <summary>
        /// Formats the error message.
        /// </summary>
        /// <param name="attributeType">Type of missing attribute.</param>
        /// <returns>Formatted error message.</returns>
        private static string FormatMessage([NotNull] Type attributeType)
        {
            return $"Missing attribute of type {attributeType.FullName}";
        }
    }
}