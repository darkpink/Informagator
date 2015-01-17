using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Stages
{
    public abstract class ProcessingStage
    {
        protected IMessageErrorHandler ErrorHandler { get; set; }

        public ProcessingStage(IMessageErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;
        }

        public string Name { get; set; }
        
        public IMessage Process(IMessage message)
        {
            IMessage result;

            try
            {
                result = Perform(message);
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex is MessageValidationException && ErrorHandler is IMessageValidationErrorHandler)
                    {
                        ((IMessageValidationErrorHandler)ErrorHandler).Handle(message, (MessageValidationException)ex);
                    }
                    else if (ex is MessageException && ErrorHandler is IMessageErrorHandler)
                    {
                        ((IMessageErrorHandler)ErrorHandler).Handle(message, (InformagatorException)ex);
                    }
                    else
                    {
                        ErrorHandler.Handle(message, ex);
                    }
                }
                catch { }

                if (ex is MessageException)
                {
                    result = null;
                }
                else
                {
                    throw;
                }
            }

            return result;
        }

        protected abstract IMessage Perform(IMessage message);
    }
}
