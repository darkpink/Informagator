using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Messages
{
    public class AsciiStringMessage : MessageBase<string>, IMessage<string>
    {
        public override byte[] BinaryData
        {
            get
            {
                return Encoding.ASCII.GetBytes(Body);
            }
            set
            {
                Body = Encoding.ASCII.GetString(value);
            }
        }
    }
}
