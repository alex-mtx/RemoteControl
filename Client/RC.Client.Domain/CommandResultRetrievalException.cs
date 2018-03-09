using System;
using System.Runtime.Serialization;

namespace RC.Client.Domain
{
    [Serializable]
    public class CommandResultRetrievalException : Exception
    {
        public CommandResultRetrievalException()
        {
        }

        public CommandResultRetrievalException(string message) : base(message)
        {
        }

        public CommandResultRetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandResultRetrievalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
