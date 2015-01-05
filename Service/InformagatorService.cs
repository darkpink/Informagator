using Acadian.Informagator.ProdProviders;
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

namespace Acadian.Informagator.Service
{
    public partial class InformagatorService : ServiceBase
    {
        protected Informagator Informagator { get; set; }
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

            Informagator = new Informagator(
                new DatabaseConfigurationProvider(),
                new DatabaseAssemblyStore(),
                new DatabaseMessageStore(),
                new DatabaseMessageTracker()
                );
            Informagator.Start();
        }

        protected override void OnStop()
        {
            Informagator.Stop();
        }
    }
}
