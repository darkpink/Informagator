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
        public IList<IProcessingStage> Stages { get; set; }

        public ProcessingSequence()
        {
            Stages = new List<IProcessingStage>();
        }

        public virtual bool Execute()
        {
            bool result = false;
            IMessage message = null;
            
            foreach (IProcessingStage stage in Stages)
            {
                message = stage.Execute(message);
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
