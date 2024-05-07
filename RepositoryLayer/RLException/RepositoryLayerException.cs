using System;

namespace RepositoryLayer.RLException
{
    public class RepositoryLayerException : Exception
    {
        public RepositoryLayerException()
        {
        }

        public RepositoryLayerException(string message) : base(message)
        {
        }

        public RepositoryLayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
