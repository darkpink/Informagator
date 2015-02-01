using Informagator.CommonComponents.Tracking;
using Informagator.Contracts;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Workers
{
    public class StageSequence
    {
        protected IList<IProcessingStage> Stages { get; set; }
        
        protected IList<IList<IMessageErrorHandler>> ErrorHandlers { get; set; }

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

        public StageSequence()
        {
            Stages = new List<IProcessingStage>();
            ErrorHandlers = new List<IList<IMessageErrorHandler>>();
        }

        public void AddStage(IProcessingStage stage, IList<IMessageErrorHandler> errorHandlers)
        {
            Stages.Add(stage);
            ErrorHandlers.Add(errorHandlers);
        }

        //TODO - instead of returning bool, return an enum indicating success, error, or no message
        public virtual bool TryProcessMessage()
        {
            bool result = false;

            List<IMessage> messagesInProcess = null;
            ProcessingSequenceTracker tracker = null;
            int currentStageIndex = 0;
            IMessage initialMessage = null;

            while(currentStageIndex == 0 || messagesInProcess != null)
            {
                var stage = Stages[currentStageIndex];
                tracker.BeginStage(stage.Name);

                try
                {
                    if (stage is ITransformStage)
                    {
                        messagesInProcess = ProcessTransformStage((ITransformStage)stage, tracker, messagesInProcess);
                    }
                    else if (stage is IObserverStage)
                    {
                        ProcessObserverStage((IObserverStage)stage, messagesInProcess);
                    }
                    else if (stage is IConsumerStage)
                    {
                        ProcessConsumerStage((IConsumerStage)stage, messagesInProcess, tracker);
                        messagesInProcess = null;
                    }
                    else if (stage is ISupplierStage)
                    {
                        initialMessage = ProcessSupplierStage((ISupplierStage)stage);
                        if (initialMessage != null)
                        {
                            result = true;
                            messagesInProcess = new List<IMessage>() { initialMessage };
                            tracker = new ProcessingSequenceTracker(MessageTracker);
                            tracker.TrackInitialMessage(initialMessage, SupplierStage.Name);
                        }
                    }
                    else
                    {
                        throw new InformagatorException(String.Format("Stage type not implemented/supported in StageSequence: {0}", stage.GetType()));
                    }
                }catch(Exception ex)
                {
                    //TODO - find a way to identify the specific message in the batch that caused the error
                    //TODO - cool idea - even if tracking is off, can we save all would-have-been tracking info
                    //       if an exception occurs :)
                    InvokeErrorHandlers(ErrorHandlers[currentStageIndex], stage.Name, initialMessage, ex);
                    if (ex is InformagatorException)
                    {
                        switch(((InformagatorException)ex).SuggestedAction)
                        {
                            case Contracts.Exceptions.Action.GotoNextMessage:
                                messagesInProcess = null;
                                break;
                            case Contracts.Exceptions.Action.GotoNextStage:
                                break;
                            default:
                                throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                currentStageIndex++;
            }

            return result;
        }

        private void InvokeErrorHandlers(IList<IMessageErrorHandler> handlers, string stageName, IMessage message, Exception ex)
        {
            foreach(IMessageErrorHandler handler in handlers)
            {
                try
                {
                    handler.Handle(new[] {"Error processing stage " + stageName}, ex, message);
                }
                catch { }
            }
        }

        private void ProcessConsumerStage(IConsumerStage stage, List<IMessage> messagesInProcess, ProcessingSequenceTracker tracker)
        {
            foreach (IMessage mip in messagesInProcess)
            {
                tracker.BeginInputMessage(mip);
                stage.Consume(mip);
                tracker.TrackSequenceOutputMessage(mip);
            }
        }

        private static void ProcessObserverStage(IObserverStage stage, List<IMessage> messagesInProcess)
        {
            foreach (IMessage mip in messagesInProcess)
            {
                stage.Observe(mip);
            }
        }

        private static List<IMessage> ProcessTransformStage(ITransformStage stage, ProcessingSequenceTracker tracker, List<IMessage> messagesInProcess)
        {
            var newMessagesInProcess = new List<IMessage>(messagesInProcess.Count);
            foreach (IMessage mip in messagesInProcess)
            {
                tracker.BeginInputMessage(mip);
                var newMessages = stage.TransformMessage(mip);
                foreach (IMessage newMessage in newMessages)
                {
                    tracker.TrackSequenceOutputMessage(newMessage);
                    newMessagesInProcess.Add(newMessage);
                }
            }

            messagesInProcess = newMessagesInProcess;
            return messagesInProcess;
        }

        private IMessage ProcessSupplierStage(ISupplierStage stage)
        {
            IMessage result;

            result = stage.Supply();

            return result;
        }
    }
}
