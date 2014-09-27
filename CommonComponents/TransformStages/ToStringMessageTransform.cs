﻿using Acadian.Informagator.Contracts;
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
        IEnumerable<IMessage> ITransformStage.TransformMessage(IMessage message)
        {
            AsciiStringMessage result;

            result = new AsciiStringMessage();
            result.BinaryData = message.BinaryData;

            return new IMessage[] { result };
        }

        public string Name
        {
            get { return "ToStringMessageTransform"; }
        }
    }
}
