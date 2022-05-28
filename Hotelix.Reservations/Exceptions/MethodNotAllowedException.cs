using System;

namespace Hotelix.Reservations.Exceptions
{
    [Serializable]
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException() : base() {}
        public MethodNotAllowedException(string message) : base(message) {}
        public MethodNotAllowedException(string message, Exception inner) : base(message, inner) {}
        protected MethodNotAllowedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}