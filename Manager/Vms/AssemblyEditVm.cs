using Informagator.Manager.Commands;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Vms
{
    public class AssemblyEditVm : EntityEditVmBase<Assembly>
    {
        public AssemblyEditVm()
            : base()
        {
            LoadAssembly = new LoadAssemblyAndDebuggingSymbolsCommand(fileName => LoadBinaries(fileName));
        }
        public ICommand LoadAssembly { get; set; }
        protected override Assembly LoadEntity()
        {
            return Entities.Assemblies
                           .Single(av => av.Id == EntityId);
            
        }

        protected override Assembly CreateNewEntity()
        {
            Assembly result = Entities.Assemblies.Create();
            Entities.Assemblies.Add(result);
            var configuration = Entities.SystemConfigurations.Single(c => c.Description == ConfigurationSelection.SelectedConfiguration);
            result.SystemConfiguration = configuration;
            return result;
        }

        protected override bool IsValid
        {
            get { return true; }
        }

        private void LoadBinaries(string fileName)
        {
            Entity.Name = Path.GetFileName(fileName);

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] assemblyBinary = new byte[stream.Length];
                stream.Read(assemblyBinary, 0, (int)stream.Length);
                Entity.Executable = assemblyBinary;
                System.Reflection.Assembly bin = System.Reflection.Assembly.ReflectionOnlyLoad(assemblyBinary);
                Entity.Version = bin.GetName().Version.ToString();
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
                return Entity.Name;
            }
        }

        public string AssemblyDotNetVersion
        {
            get
            {
                return Entity.Version;
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
