using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Informagator.CommonComponents.Messages
{
    public class XmlMessage : MessageBase<XmlDocument>
    {
        public override byte[] BinaryData
        {
            get
            {
                byte[] result;

                if (Body != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        Body.Save(stream);
                        result = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(result, 0, (int)stream.Length);
                    }
                }
                else
                {
                    result = null;
                }
                return result;
            }
            set
            {
                
            }
        }
    }
}
