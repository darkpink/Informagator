using Informagator.CommonComponents.Messages;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
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
        [ConfigurationParameter]
        public string BodyType { get; set; }

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
            Dictionary<DataColumn, PropertyInfo> columnPropertyMapping = GetColumnPropertyMapping(dataTable);

            foreach(DataRow row in dataTable.Rows.OfType<DataRow>())
            {
                IMessage objectMessage = CreateObjectMessage();
            }

            return messages;
        }

        private Dictionary<DataColumn, PropertyInfo> GetColumnPropertyMapping(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        private IMessage CreateObjectMessage()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public void ValidateSettings()
        {
        }

        public object ObjectMessage { get; set; }
    }
}
