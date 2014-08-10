using System;
using System.Runtime.Serialization;

namespace NetatmoBot.Exceptions
{
    [Serializable]
    public class NetatmoReadException : Exception
    {
        public NetatmoReadException()
        { }

        public NetatmoReadException(string message)
            : base(message)
        { }

        public NetatmoReadException(string message, Exception inner)
            : base(message, inner)
        { }

        protected NetatmoReadException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}