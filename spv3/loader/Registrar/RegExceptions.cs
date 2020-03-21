using System;

namespace Registrar
{
    public class RegOptionRegistrationException : Exception
    {
        public RegOptionRegistrationException() { }
        public RegOptionRegistrationException(string message) : base(message) { }
        public RegOptionRegistrationException(string message, Exception inner) : base(message, inner) { }
        protected RegOptionRegistrationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegOptionRetrievalException : Exception
    {
        public RegOptionRetrievalException() { }
        public RegOptionRetrievalException(string message) : base(message) { }
        public RegOptionRetrievalException(string message, Exception inner) : base(message, inner) { }
        protected RegOptionRetrievalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegOptionAssignmentException : Exception
    {
        public RegOptionAssignmentException() { }
        public RegOptionAssignmentException(string message) : base(message) { }
        public RegOptionAssignmentException(string message, Exception inner) : base(message, inner) { }
        protected RegOptionAssignmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegLoadException : Exception
    {
        public RegLoadException() { }
        public RegLoadException(string message) : base(message) { }
        public RegLoadException(string message, Exception inner) : base(message, inner) { }
        protected RegLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegSaveException : Exception
    {
        public RegSaveException() { }
        public RegSaveException(string message) : base(message) { }
        public RegSaveException(string message, Exception inner) : base(message, inner) { }
        protected RegSaveException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegConversionException : Exception
    {
        public RegConversionException() { }
        public RegConversionException(string message) : base(message) { }
        public RegConversionException(string message, Exception inner) : base(message, inner) { }
        protected RegConversionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}