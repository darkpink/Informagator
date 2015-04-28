using Informagator.CommonComponents.Messages;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using Informagator.HL7Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxCustom
{
    public class TestHL7Transform : ITransformStage
    {
        public IEnumerable<Informagator.Contracts.IMessage> TransformMessage(Informagator.Contracts.IMessage message)
        {
            HL7Message msg = new HL7Message();

            if (!(message is ObjectMessage<TestMessageBody>))
            {
                throw new MessageException("ObjectMessage<TestMessageBody> is expected in TestHL7Transform");
            }
            TestMessageBody messageIn = ((ObjectMessage<TestMessageBody>)message).Body;
            
            msg["MSH"][3] = "sender";
            msg["MSH"][12] = messageIn.Description;
            msg.AddSegments("EVN", "PID");
            msg["EVN"][5] = messageIn.Value;
            msg["PID"][2] = messageIn.Id.ToString();


            return new[] { msg };
        }

        public string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        public void ValidateSettings()
        {
        }
    }
}
