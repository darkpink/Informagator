using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Messages
{
    public class ObjectMessage<TBody> : MessageBase<TBody>
    {
        protected BinaryFormatter Formatter { get; set; }

        public override byte[] BinaryData
        {
            get
            {
                byte[] result;
                using (MemoryStream stream = new MemoryStream())
                {
                    Formatter.Serialize(stream, Body);
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ObjectMessage()
        {
            Formatter = new BinaryFormatter();
        }
    }
}
