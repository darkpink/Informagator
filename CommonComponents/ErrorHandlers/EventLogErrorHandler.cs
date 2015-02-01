using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ErrorHandlers
{
    //TODO: allow event id and category to be specified in config,
    //also better formatting for the data in the log
    public class EventLogErrorHandler : IMessageErrorHandler
    {
        protected const string Log = "Application";
        protected EventLog LocalEventLog { get; set; }

        private bool _sourceExists;
        protected bool SourceExists 
        { 
            get
            {
                if (!_sourceExists && !String.IsNullOrWhiteSpace(Source))
                {
                    _sourceExists = EventLog.SourceExists(Source);
                }
                return _sourceExists;
            }
        }

        [ConfigurationParameter]
        public string Source { get; set;}

        public EventLogErrorHandler()
        {
            EventLog log = new EventLog(Log);
        }

        public void Handle(IList<string> info, Exception ex, IMessage message)
        {
            if (!String.IsNullOrWhiteSpace(Source))
            {
                if (!SourceExists)
                {
                    EventLog.CreateEventSource(Source, Log);
                }

                EventLog.WriteEntry(Source, info + ex.ToString(), EventLogEntryType.Warning, 0, (short)0, message.BinaryData);
            }
        }


        public void ValidateSettings()
        {
            //TODO
        }

        public IList<string> ContextInfo { get; set;}
    }
}
