using System;

namespace Hotelix.Reservations.Exceptions
{
    [Serializable]
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() : base() {}
        public ServiceUnavailableException(string message) : base(message) {}
        public ServiceUnavailableException(string message, Exception inner) : base(message, inner) {}
        protected ServiceUnavailableException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}