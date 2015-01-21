using Informagator.ProdProviders;
using Informagator.Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Informagator.Contracts;

namespace Informagator.Service
{
    public partial class InformagatorService : ServiceBase
    {
        protected IMachine Informagator { get; set; }
        public InformagatorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string newWorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (newWorkingDirectory.StartsWith(@"file:\"))
            {
                newWorkingDirectory = newWorkingDirectory.Substring(6);
            }
            Directory.SetCurrentDirectory(newWorkingDirectory);

            string machineNameOverride = args.Length > 0 ? args[0] : null;

            Informagator = new DefaultMachine(machineNameOverride);
            Informagator.Start();
        }

        protected override void OnStop()
        {
            Informagator.Stop();
        }
    }
}
