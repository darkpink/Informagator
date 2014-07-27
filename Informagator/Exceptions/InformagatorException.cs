﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Exceptions
{
    [Serializable]
    public class InformagatorException : Exception
    {
        public InformagatorException()
            : base()
        {
        }

        public InformagatorException(string message)
            : base(message)
        {
        }

        public InformagatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InformagatorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}