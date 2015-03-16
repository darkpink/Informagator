using Informagator.Contracts;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ReplyBuilderStages
{
    public class EchoReplyBuilder : IReplyBuilderStage
    {
        public IMessage SupplyReply(IMessage message)
        {
            return message;
        }

        public string Name
        {
            get { return "Echo Reply Builder"; }
        }

        public void ValidateSettings()
        {
        }
    }
}
