using Informagator.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Messages
{
    public class ObjectMessage<TBody> : MessageBase<TBody>
        where TBody : class, new()
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
                    result = new byte[stream.Length];
                    stream.Read(result, 0, result.Length);
                }
                
                return result;
            }
            set
            {
                using (MemoryStream stream = new System.IO.MemoryStream(value))
                {
                    Body = (TBody)Formatter.Deserialize(stream);
                }
            }
        }

        public ObjectMessage()
        {
            Formatter = new BinaryFormatter();
        }

        public ObjectMessage(TBody body)
        {
            Formatter = new BinaryFormatter();
            Body = body;
        }
    }
}
