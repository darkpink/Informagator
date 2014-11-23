using Acadian.Informagator.Manager.Commands;
using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Acadian.Informagator.Manager.Vms
{
    public class AssemblyEditVm : EntityEditVmBase<AssemblyVersion>
    {
        public AssemblyEditVm()
            : base()
        {
            LoadAssembly = new LoadAssemblyAndDebuggingSymbolsCommand(fileName => LoadBinaries(fileName));
        }
        public ICommand LoadAssembly { get; set; }
        protected override AssemblyVersion LoadEntity()
        {
            return Entities.AssemblyVersions
                           .Single(av => av.Id == EntityId);
            
        }

        protected override AssemblyVersion CreateNewEntity()
        {
            AssemblyVersion result = Entities.AssemblyVersions.Create();
            Entities.AssemblyVersions.Add(result);
            var configuration = Entities.SystemConfigurations.Single(c => c.Description == SelectedConfiguration);
            var assemblySystemConfiguration = Entities.AssemblySystemConfigurations.Create();
            Entities.AssemblySystemConfigurations.Add(assemblySystemConfiguration);
            assemblySystemConfiguration.AssemblyVersion = result;
            result.AssemblySystemConfigurations.Add(assemblySystemConfiguration);
            assemblySystemConfiguration.SystemConfiguration = configuration;
            return result;
        }

        protected override bool IsValid
        {
            get { return true; }
        }

        private void LoadBinaries(string fileName)
        {
            Entity.AssemblyName = Path.GetFileName(fileName);
            Entity.AssemblySystemConfigurations.ToList().ForEach(asc => asc.AssemblyName = Entity.AssemblyName);

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] assemblyBinary = new byte[stream.Length];
                stream.Read(assemblyBinary, 0, (int)stream.Length);
                Entity.Executable = assemblyBinary;
                System.Reflection.Assembly bin = System.Reflection.Assembly.ReflectionOnlyLoad(assemblyBinary);
                Entity.AssemblyDotNetVersion = bin.GetName().Version.ToString();
                Entity.AssemblySystemConfigurations.ToList().ForEach(asc => asc.AssemblyDotNetVersion = Entity.AssemblyDotNetVersion);
                Entity.LoadDttm = DateTime.Now;
                NotifyPropertyChanged("AssemblyByteCount");
                NotifyPropertyChanged("AssemblyName");
                NotifyPropertyChanged("AssemblyDotNetVersion");
                NotifyPropertyChanged("LoadDttm");
            }

            string debuggingSymbolFile = Path.ChangeExtension(fileName, ".pdb");
            byte[] debuggingSymbolsBinary = null;
            if (File.Exists(debuggingSymbolFile))
            {
                using (FileStream stream = new FileStream(debuggingSymbolFile, FileMode.Open, FileAccess.Read))
                {
                    debuggingSymbolsBinary = new byte[stream.Length];
                    stream.Read(debuggingSymbolsBinary, 0, (int)stream.Length);
                }
            }
            Entity.DebuggingSymbols = debuggingSymbolsBinary;
            NotifyPropertyChanged("DebuggingSymbolByteCount");
        }

        public DateTime? LoadDttm
        {
            get
            {
                return Entity.LoadDttm;
            }
        }

        public string AssemblyName
        {
            get
            {
                return Entity.AssemblyName;
            }
        }

        public string AssemblyDotNetVersion
        {
            get
            {
                return Entity.AssemblyDotNetVersion;
            }
        }
        public int? AssemblyByteCount
        {
            get
            {
                return Entity.Executable == null ? (int?)null : Entity.Executable.Length;
            }
        }

        public int? DebuggingSymbolByteCount
        {
            get
            {
                return Entity.DebuggingSymbols == null ? (int?)null : Entity.DebuggingSymbols.Length;
            }
        }
    }
}
