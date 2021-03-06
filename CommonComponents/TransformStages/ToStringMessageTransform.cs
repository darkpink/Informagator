﻿using Informagator.CommonComponents.Messages;
using Informagator.Contracts;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.TransformStages
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


        public void ValidateSettings()
        {
        }
    }
}
