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

        protected IMessageTracker MessageTracker { get; set; }

        public StageSequence(IMessageTracker messageTracker)
        {
            Stages = new List<IProcessingStage>();
            ErrorHandlers = new List<IList<IMessageErrorHandler>>();
            MessageTracker = messageTracker;
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
            StageSequenceTracker tracker = null;
            int currentStageIndex = 0;
            IMessage initialMessage = null;

            while(currentStageIndex < Stages.Count && (currentStageIndex == 0 || messagesInProcess != null))
            {
                var stage = Stages[currentStageIndex];
                if (currentStageIndex > 0)
                {
                    tracker.BeginNonInitialStage(stage.Name);
                }

                try
                {
                    if (stage is IReplyBuilderStage)
                    {
                        ProcessReplyBuilderStage((IReplyingSupplierStage)Stages[0], (IReplyBuilderStage)stage, messagesInProcess, tracker);
                    }
                    else if (stage is ITransformStage)
                    {
                        messagesInProcess = ProcessTransformStage((ITransformStage)stage, tracker, messagesInProcess);
                    }
                    else if (stage is IObserverStage)
                    {
                        ProcessObserverStage((IObserverStage)stage, messagesInProcess, tracker);
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
                            tracker = new StageSequenceTracker(MessageTracker);
                            tracker.TrackInitialSupplierStageOutput(initialMessage, SupplierStage.Name);
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

            if (Stages[0] is IReplyingSupplierStage)
            {
                ((IReplyingSupplierStage)Stages[0]).Consumed();
            }

            return result;
        }

        private void ProcessReplyBuilderStage(IReplyingSupplierStage supplierStage, IReplyBuilderStage builderStage, List<IMessage> messagesInProcess, StageSequenceTracker tracker)
        {
            List<IMessage> replies = new List<IMessage>();

            foreach(IMessage mip in messagesInProcess)
            {
                IMessage reply = null;

                try
                {
                    tracker.BeginNonInitialStageInputMessage(mip);
                    reply = builderStage.SupplyReply(mip);
                    tracker.TrackNonInitialStageOutputMessage(reply);
                    replies.Add(reply);
                }catch(Exception ex)
                {
                    tracker.TrackStageException(builderStage.Name, mip, ex);
                    throw;
                }
            }

            foreach (IMessage mip in replies)
            {
                try
                {
                    tracker.TrackReplyToInitialSupplierStage(supplierStage.Name, mip);
                    supplierStage.Reply(mip);
                }
                catch (Exception ex)
                {
                    tracker.TrackStageException(builderStage.Name, mip, ex);
                    throw;
                }
            }
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

        private void ProcessConsumerStage(IConsumerStage stage, List<IMessage> messagesInProcess, StageSequenceTracker tracker)
        {
            foreach (IMessage mip in messagesInProcess)
            {
                tracker.BeginNonInitialStageInputMessage(mip);
                try
                {
                    stage.Consume(mip);
                    tracker.TrackNoninitialStageOutputMessage(mip);
                }
                catch (Exception ex)
                {
                    tracker.TrackStageException(stage.Name, mip, ex);
                    throw;
                }
            }
        }

        private static void ProcessObserverStage(IObserverStage stage, List<IMessage> messagesInProcess, StageSequenceTracker tracker)
        {
            foreach (IMessage mip in messagesInProcess)
            {
                try
                {
                    stage.Observe(mip);
                }
                catch(Exception ex)
                {
                    tracker.TrackStageException(stage.Name, mip, ex);
                    throw;
                }
            }
        }

        private static List<IMessage> ProcessTransformStage(ITransformStage stage, StageSequenceTracker tracker, List<IMessage> messagesInProcess)
        {
            var newMessagesInProcess = new List<IMessage>(messagesInProcess.Count);
            foreach (IMessage mip in messagesInProcess)
            {
                tracker.BeginNonInitialStageInputMessage(mip);
                var newMessages = stage.TransformMessage(mip);
                foreach (IMessage newMessage in newMessages)
                {
                    tracker.TrackNoninitialStageOutputMessage(newMessage);
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
