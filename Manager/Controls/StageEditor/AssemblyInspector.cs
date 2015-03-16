using Informagator.Contracts.Attributes;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Controls.StageEditor
{
    public class AssemblyInspector : MarshalByRefObject
    {
        protected string SelectedSystemConfiguration { get; set; }

        public List<StageParameter> Inspect(string selectedSystemConfiguration, byte[] toReflect, string type)
        {
            List<StageParameter> result = new List<StageParameter>();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load(toReflect);

            Type t = asm.GetType(type);
            var propsWithAttribute = t.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType.FullName == typeof(ConfigurationParameterAttribute).FullName));
            foreach (PropertyInfo info in propsWithAttribute)
            {
                ConfigurationParameterAttribute attr = (ConfigurationParameterAttribute)info.GetCustomAttributes().Single(a => a.GetType() == typeof(ConfigurationParameterAttribute));
                string displayName = attr.DisplayName ?? info.Name;
                Type propType = info.PropertyType;
                result.Add(new StageParameter() { DisplayName = displayName, Name = info.Name, PropertyType = propType });
            }

            return result;
        }

        protected System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly result = null;

            var asmName = new System.Reflection.AssemblyName(args.Name);
            string n = asmName.Name + ".dll";
            string v = asmName.Version.ToString();

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                byte[] assemblyBinary = entities.Assemblies
                                        .Where(av => av.Name == n &&
                                                     av.Version == v &&
                                                     av.SystemConfiguration.Description == SelectedSystemConfiguration
                                               )
                                         .Select(av => av.Executable)
                                         .SingleOrDefault();
                result = System.Reflection.Assembly.Load(assemblyBinary);
            }

            return result;
        }
    }
}
