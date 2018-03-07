using System;
using System.Runtime.Serialization;

namespace RC.Client.Domain
{
    public class CmdExecutionException : Exception
    {
        public CmdExecutionException()
        {
        }

        public CmdExecutionException(string message) : base(message)
        {
        }

        public CmdExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CmdExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
