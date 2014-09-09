using Acadian.Informagator.Contracts;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.TransformStages
{
    public class ToStringMessageTransform : ITransformStage
    {
        public IMessage TransformMessage(IMessage message)
        {
            AsciiStringMessage result;

            result = new AsciiStringMessage();
            result.BinaryData = message.BinaryData;

            return result;
        }
    }
}
