using System;

namespace FundooAPI.Exceptions
{
    public class RabbitMQProducerException : Exception
    {
        public RabbitMQProducerException() { }

        public RabbitMQProducerException(string message)
            : base(message) { }

        public RabbitMQProducerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
