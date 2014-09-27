using Acadian.Informagator.Contracts;
using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Tracking;
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

        public IMessageTracker MessageTracker { get; set; }

        protected Guid CurrentSequenceId { get; set; }

        public ProcessingSequence()
        {
            Stages = new List<IProcessingStage>();
        }

        public virtual bool Execute()
        {
            bool result = false;
            IMessage initialMessage = GetMessageFromSupplier();

            if (initialMessage != null)
            {
                result = true;
                CurrentSequenceId = Guid.NewGuid();
                TrackingInfo initialTrackingInfo = GetInitialTrackingInfo();
                MessageTracker.TrackMessage(initialTrackingInfo, initialMessage);

                IEnumerable<IMessage> messagesInProcess = new[] {initialMessage};

                foreach (IProcessingStage stage in Stages.Skip(1).Take(Stages.Count - 2))
                {
                    if (stage is ITransformStage)
                    {
                        List<IMessage> newMessagesInProcess = new List<IMessage>();
                        
                        foreach(IMessage mip in messagesInProcess)
                        {
                           newMessagesInProcess.AddRange(((ITransformStage)stage).TransformMessage(initialMessage));
                        }
                        
                        messagesInProcess = newMessagesInProcess;
                    }
                    else if (stage is IObserverStage)
                    {
                        foreach(IMessage mip in messagesInProcess)
                        {
                            ((IObserverStage)stage).Observe(initialMessage);
                        }
                    }
                    else
                    {
                        throw new ConfigurationException(String.Format("Only transform or observer stages are allowed in the middle of processing sequences. Type {0} is not allowed.", stage.GetType()));
                    }
                }

                IConsumerStage consumerStage = (IConsumerStage)Stages.Last();
                foreach (IMessage mip in messagesInProcess)
                {
                    consumerStage.Consume(mip);
                }
            }
            
            return result;
        }

        private TrackingInfo GetInitialTrackingInfo()
        {
            TrackingInfo result = new TrackingInfo();

            ISupplierStage supplierStage = (ISupplierStage)Stages.First();
            result.ReceivedFrom = supplierStage.ReceviedFrom;
            result.SentTo = Stages.Skip(1).First().Name;
            result.StageSequence = 0;
            result.Stage = supplierStage.Name;
            result.TrackDateTime = DateTime.Now;
            result.ProcessingSequenceId = CurrentSequenceId;

            return result;
        }

        private IMessage GetMessageFromSupplier()
        {
            IMessage result;

            ISupplierStage supplierStage = (ISupplierStage)Stages.First();
            result = supplierStage.Supply();

            return result;
        }
    }
}
