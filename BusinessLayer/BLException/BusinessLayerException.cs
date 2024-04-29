using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BLException
{
    public class BusinessLayerException:Exception
    {
        public BusinessLayerException()
        {
        }

        public BusinessLayerException(string message) : base(message)
        {
        }

        public BusinessLayerException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
