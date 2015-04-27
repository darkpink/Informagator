using Informagator.CommonComponents.Messages;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.TransformStages
{
    public class DataTableToObjectTransform : ITransformStage
    {
        [HostProvided]
        public IAssemblyManager AssemblyManager { get; set; }

        [ConfigurationParameter(DisplayName="Body Type Assembly")]
        public string BodyAssembly { get; set; }

        [ConfigurationParameter(DisplayName="Body Type Assembly Version")]
        public string BodyAssemblyVersion { get; set; }

        [ConfigurationParameter(DisplayName="Body Type")]
        public string BodyTypeName { get; set; }

        [ConfigurationParameter(DisplayName = "Always Same Columns")]
        public bool IsAlwaysSameColumns { get; set; }

        protected Type BodyType { get; set; }

        protected Type MessageType { get; set; }

        protected Dictionary<string, PropertyInfo> ColumnPropertyMapping { get; set; }

        protected Dictionary<string, Func<object, object>> ColumnConversions { get; set; }

        public IEnumerable<IMessage> TransformMessage(IMessage message)
        {
            List<IMessage> result = new List<IMessage>();

            if (!(message is ObjectMessage<DataTable>))
            {
                throw new ConfigurationException("DataTableObjectTransform must be passed an ObjectMessage<DataTable>");
            }

            result = CovertDataTableToObject(((ObjectMessage<DataTable>)message).Body);
            
            return result;
        }

        private List<IMessage> CovertDataTableToObject(DataTable dataTable)
        {
            int rowCount = dataTable.Rows.Count;
            List<IMessage> messages = new List<IMessage>(rowCount);
            Dictionary<string, PropertyInfo> mapping = GetColumnPropertyMappings(dataTable);
            Dictionary<string, Func<object, object>> conversions = GetColumnConversions(dataTable);

            foreach(DataRow row in dataTable.Rows.OfType<DataRow>())
            {
                IMessage objectMessage = CreateObjectMessage();
            }

            return messages;
        }

        private Dictionary<string, Func<object, object>> GetColumnConversions(DataTable dataTable)
        {
            Dictionary<string, Func<object, object>> result;

            if (IsAlwaysSameColumns && ColumnConversions != null)
            {
                result = ColumnConversions;
            }
            else
            {
                result = new Dictionary<string, Func<object, object>>();
                ColumnConversions = result;
                PropertyInfo[] properties = BodyType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

                foreach (DataColumn col in dataTable.Columns)
                {
                    string colname = col.ColumnName;
                    PropertyInfo matchingProperty = properties.FirstOrDefault(p => String.Compare(p.Name, colname, true) == 0);
                    if (matchingProperty != null)
                    {
                        if (col.DataType != matchingProperty.PropertyType)
                        {
                            Func<object, object> conversion = objIn => Convert.ChangeType(objIn, matchingProperty.PropertyType);
                            result.Add(col.ColumnName, conversion);
                        }
                    }
                }
            }
            
            return result;
        }

        private Dictionary<string, PropertyInfo> GetColumnPropertyMappings(DataTable dataTable)
        {
            Dictionary<string, PropertyInfo> result;

            if (IsAlwaysSameColumns && ColumnPropertyMapping != null)
            {
                result = ColumnPropertyMapping;
            }
            else
            {
                result = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] properties = BodyType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                foreach (DataColumn col in dataTable.Columns)
                {
                    string colname = col.ColumnName;
                    PropertyInfo matchingProperty = properties.FirstOrDefault(p => String.Compare(p.Name, colname, true) == 0);
                    if (matchingProperty != null)
                    {
                        result.Add(colname, matchingProperty);
                    }
                }
            }

            return result;
        }

        private IMessage CreateObjectMessage()
        {
            IMessage objectMessage = Activator.CreateInstance(MessageType) as IMessage;
            return objectMessage;
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public void ValidateSettings()
        {
            Assembly bodyTypeAssembly = AssemblyManager.GetAssembly(BodyAssembly, BodyAssemblyVersion);
            BodyType = bodyTypeAssembly.GetType(BodyTypeName);
            object body = Activator.CreateInstance(BodyType, false);
            MessageType = typeof(ObjectMessage<>).MakeGenericType(BodyType);
        }

        public object ObjectMessage { get; set; }
    }
}
