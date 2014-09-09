using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Messages
{
    public class ByteArrayMessage : MessageBase<byte[]>, IMessage
    {
        public ByteArrayMessage()
        {
        }

        public ByteArrayMessage(Stream data)
        {
            Body = new byte[data.Length];
            data.Read(Body, 0, (int)data.Length);
        }
        public override byte[] BinaryData
        {
            get
            {
                return Body;
            }
            set
            {
                Body = value;
            }
        }
    }
}
