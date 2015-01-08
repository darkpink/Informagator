using Informagator.Contracts;
using Informagator.Exceptions;
using Informagator.Messages;
using Informagator.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Stages
{
    public class ProcessingSequence
    {
        public IList<IProcessingStage> Stages { get; set; }

        protected ISupplierStage SupplierStage
        {
            get
            {
                return (ISupplierStage)(Stages.First());
            }
        }
        protected IConsumerStage ConsumerStage
        {
            get
            {
                return (IConsumerStage)(Stages.Last());
            }
        }
        protected IEnumerable<IProcessingStage> IntermediateStages
        {
            get
            {
                return Stages.Skip(1).Take(Stages.Count - 2);
            }
        }

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
                ProcessingSequenceTracker tracker = new ProcessingSequenceTracker(CurrentSequenceId, MessageTracker);
                tracker.TrackInitialMessage(initialMessage, SupplierStage.Name);

                List<IMessage> messagesInProcess = new List<IMessage>() { initialMessage };

                foreach(IProcessingStage stage in IntermediateStages)
                {
                    tracker.BeginStage(stage.Name);

                    if (stage is ITransformStage)
                    {
                        var newMessagesInProcess = new List<IMessage>(messagesInProcess.Count);
                        foreach(IMessage mip in messagesInProcess)
                        {
                            tracker.BeginInputMessage(mip);
                            var newMessages = ((ITransformStage)stage).TransformMessage(initialMessage);
                            foreach(IMessage newMessage in newMessages)
                            {
                                tracker.TrackSequenceOutputMessage(newMessage);
                                newMessagesInProcess.Add(newMessage);
                            }
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

                tracker.BeginStage(ConsumerStage.Name);
                foreach (IMessage mip in messagesInProcess)
                {
                    tracker.BeginInputMessage(mip);
                    ConsumerStage.Consume(mip);
                    tracker.TrackSequenceOutputMessage(mip);
                }
            }
            
            return result;
        }

        private TrackingInfo TrackInitialMessage(IMessage initialMessage)
        {
            TrackingInfo result = new TrackingInfo(CurrentSequenceId, 0, SupplierStage.Name, 0);
            MessageTracker.TrackMessage(result, initialMessage);
            return result;
        }

        private TrackingInfo TrackMessage(TrackingInfo previousTrackingInfo, string stage, int messageSequence, IMessage intermediateMessage)
        {
            TrackingInfo result = previousTrackingInfo.GetNextInSequence(stage, messageSequence);
            MessageTracker.TrackMessage(result, intermediateMessage);
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
