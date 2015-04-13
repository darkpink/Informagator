using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Informagator.Manager.Controls;

namespace Informagator.Manager.Vms
{
    public class ErrorHandlerEditVm : EntityEditVmBase<ErrorHandler>
    {
        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
                NotifyPropertyChanged("Parameters");
            }
        }
        protected override bool IsValid
        {
            get { return true; }
        }
        
        protected override ErrorHandler LoadEntity()
        {
            var ErrorHandler = Entities.ErrorHandlers.Include(eh => eh.ErrorHandlerParameters)
                                         .Single(eh => eh.Id == EntityId);
            AssemblyId = ErrorHandler.Assembly.Id;
            LoadParameters(ErrorHandler);
            return ErrorHandler;
        }

        private void LoadParameters(ErrorHandler errorHandler)
        {
            Parameters.Clear();

            foreach (ErrorHandlerParameter param in errorHandler.ErrorHandlerParameters)
            {
                Parameter editorParam = new Parameter();
                editorParam.Name = param.Name;
                editorParam.Value = param.Value;
                Parameters.Add(editorParam);
            }
        }

        private void SaveParameters()
        {
            while(Entity.ErrorHandlerParameters.Count < Parameters.Count)
            {
                ErrorHandlerParameter newParam = new ErrorHandlerParameter() { ErrorHandler = Entity };
                Entities.ErrorHandlerParameters.Add(newParam);
                Entity.ErrorHandlerParameters.Add(newParam);
            }

            foreach(ErrorHandlerParameter param in Entity.ErrorHandlerParameters.Skip(Parameters.Count).ToList())
            {
                Entities.ErrorHandlerParameters.Remove(param);
            }

            for(int index = 0; index < Parameters.Count; index++)
            {
                ErrorHandlerParameter dbParam = Entity.ErrorHandlerParameters.ElementAt(index);
                Parameter uiStage = Parameters[index];
                dbParam.Name = uiStage.Name;
                dbParam.Value = uiStage.Value;
            }
        }

        protected override ErrorHandler CreateNewEntity()
        {
            var ErrorHandler = Entities.ErrorHandlers.Create();
            Entities.ErrorHandlers.Add(ErrorHandler);
            ErrorHandler.SystemConfiguration = Entities.SystemConfigurations.Single(c => c.Name == ConfigurationSelection.SelectedConfiguration);
            return ErrorHandler;
        }

        private long? _assemblyId;
        public long? AssemblyId
        {
            get
            {
                return _assemblyId;
            }
            set
            {
                _assemblyId = value;
                NotifyPropertyChanged("AssemblyId");
                AttemptToSetAssembly();
            }
        }

        public ErrorHandlerEditVm()
        {
            Parameters = new ObservableCollection<Parameter>();
        }
        public override void SaveEntity()
        {
            SaveParameters();
            base.SaveEntity();
        }
        
        private void AttemptToSetAssembly()
        {
            if (Entity != null && AssemblyId != null)
            {
                Assembly ErrorHandlerAssembly = Entities.Assemblies.SingleOrDefault(av => av.Id == AssemblyId);
                Entity.Assembly = ErrorHandlerAssembly;
            }
        }
    }
}
