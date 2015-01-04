using Acadian.Informagator.ProdProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
            Informagator = new Informagator(
                new DatabaseConfigurationProvider(),
                new DatabaseAssemblyStore(),
                new DatabaseMessageStore(),
                null
                );
            Informagator.Start();
        }

        protected override void OnStop()
        {
            Informagator.Stop();
        }
    }
}
