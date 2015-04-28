using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Machine
{
    public class DefaultAssemblyManager : IAssemblyManager
    {
        private Dictionary<string, Dictionary<string, Assembly>> LoadedAssemblies = new Dictionary<string, Dictionary<string, Assembly>>();
        private Dictionary<string, Dictionary<string, byte[]>> LoadedAssemblyBytes = new Dictionary<string, Dictionary<string, byte[]>>();

        private IAssemblyProvider AssemblyProvider { get; set; }
        
        public DefaultAssemblyManager(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
            return GetAssembly(name.Name, name.Version.ToString());
        }

        public Assembly GetAssembly(string name, string version)
        {
            Assembly result;

            Assembly[] ha = AppDomain.CurrentDomain.GetAssemblies();

            if (LoadedAssemblies.ContainsKey(name) && LoadedAssemblies[name].ContainsKey(version))
            {
                result = LoadedAssemblies[name][version];
            }
            else if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.ManifestModule.ScopeName == name && a.GetName().Version.ToString() == version))
            {
                result = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.ManifestModule.ScopeName == name);
            }
            else
            {
                byte[] assemblyBytes = AssemblyProvider.GetAssemblyBinary(name, version);
                byte[] debuggingSymbolBytes = AssemblyProvider.GetDebuggingSymbolBinary(name, version);

                if (!LoadedAssemblies.ContainsKey(name))
                {
                    LoadedAssemblies.Add(name, new Dictionary<string, Assembly>());
                    LoadedAssemblyBytes.Add(name, new Dictionary<string, byte[]>());
                }

                result = Assembly.Load(assemblyBytes, debuggingSymbolBytes);

                LoadedAssemblies[name].Add(version, result);
                LoadedAssemblyBytes[name].Add(version, assemblyBytes);
            }

            return result;
        }


        public bool AnyAssemblyChanged
        {
            get 
            {
                bool result = false;

                foreach(var assemblyNameAndVersion in LoadedAssemblies.SelectMany(kvp1 => kvp1.Value.Select(kvp2 => new { name = kvp1.Key, version = kvp2.Key })))
                {
                    byte[] existingAssemblyBytes = LoadedAssemblyBytes[assemblyNameAndVersion.name][assemblyNameAndVersion.version];
                    byte[] newAssemblyBytes = AssemblyProvider.GetAssemblyBinary(assemblyNameAndVersion.name, assemblyNameAndVersion.version);
                    if (!existingAssemblyBytes.SequenceEqual(newAssemblyBytes))
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        public object CreateConfiguredObject(IConfigurableTypeConfiguration type, object host)
        {
            object result;

            result = CreateObject(type.AssemblyName, type.AssemblyVersion, type.Type);
            ApplyConfigurationParameters(type.Parameters, result);
            ApplyHostProvidedDependencies(host, result);

            return result;
        }

        private object CreateObject(string assemblyName, string assemblyVersion, string type)
        {
            object result;

            //TODO - need good exceptions if any of these fail
            Assembly typeAssembly = GetAssembly(assemblyName, assemblyVersion);
            Type objectType = typeAssembly.GetType(type);
            result = Activator.CreateInstance(objectType);

            return result;
        }

        private void ApplyHostProvidedDependencies(object hostObject, object result)
        {
            IEnumerable<PropertyInfo> resultHostProvidedProperties = result.GetType()
                                                                           .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                                           .Where(pi => pi.GetCustomAttributes()
                                                                                          .OfType<HostProvidedAttribute>()
                                                                                          .Any());
            IEnumerable<PropertyInfo> hostPropsForResult = hostObject.GetType()
                                                                     .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                                     .Where(pi => pi.GetCustomAttributes()
                                                                                    .OfType<ProvideToClientAttribute>()
                                                                                    .Any());
            var availableDependenciesFromHost = new Dictionary<Type, object>();

            foreach (PropertyInfo pi in hostPropsForResult)
            {
                IEnumerable<ProvideToClientAttribute> attrs = pi.GetCustomAttributes<ProvideToClientAttribute>();
                object value = pi.GetValue(hostObject);
                foreach (ProvideToClientAttribute attr in attrs)
                {
                    availableDependenciesFromHost.Add(attr.InterfaceType, value);
                }
            }

            foreach (PropertyInfo pi in resultHostProvidedProperties)
            {
                if (availableDependenciesFromHost.ContainsKey(pi.PropertyType))
                {
                    pi.SetValue(result, availableDependenciesFromHost[pi.PropertyType]);
                }
            }
        }

        private void ApplyConfigurationParameters(IList<IConfigurationParameter> parameters, object result)
        {
            IEnumerable<PropertyInfo> configProps = result.GetType().GetProperties().Where(pi => pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().Any());
            foreach (PropertyInfo pi in configProps)
            {
                ConfigurationParameterAttribute[] configParams = pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().ToArray();
                IConfigurationParameter param = parameters.SingleOrDefault(p => p.Name == pi.Name);
                if (param != null)
                {
                    pi.SetValue(result, param.Value);
                }
            }
        }
    }
}
