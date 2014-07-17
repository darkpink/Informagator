using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Stages
{
    public class ProcessingSequence
    {
        public IList<ProcessingStage> Stages { get; set; }

        public ProcessingSequence()
        {
            Stages = new List<ProcessingStage>();
        }

        public virtual bool Execute()
        {
            bool result = false;
            IMessage message = null;
            
            foreach (ProcessingStage stage in Stages)
            {
                message = stage.Process(message);
                if (message == null)
                {
                    break;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
