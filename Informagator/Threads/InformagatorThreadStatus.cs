﻿using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Threads
{
    public class InformagatorThreadStatus : IInformagatorThreadStatus
    {
        public virtual string ThreadName { get; set; }
        public virtual string HostName { get; set; }
        public virtual DateTime? Initialized { get; set; }
        public virtual DateTime? RunningSince { get; set; }
        public virtual DateTime? Stopped { get; set; }
        public virtual DateTime? HeartBeat { get; set; }
        public virtual long MessageCount { get; set; }
        public virtual DateTime? LastMessage { get; set; }
        public virtual string Info { get; set; }
        public virtual bool IsRunning { get; set; }
    }
}
