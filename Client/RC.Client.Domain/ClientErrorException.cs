using System;
using System.Runtime.Serialization;

namespace RC.Client.Domain
{
    [Serializable]
    public class ClientErrorException : Exception
    {
        public ClientErrorException()
        {
        }

        public ClientErrorException(string message) : base(message)
        {
        }

        public ClientErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
