using Informagator.CommonComponents.Messages;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.SupplierStages
{
    public abstract class DataTableSupplier : ISupplierStage
    {
        [ConfigurationParameter]
        public string Server { get; set; }

        [ConfigurationParameter]
        public string Database { get; set; }
        
        protected abstract string SqlStatement { get; }

        protected SqlConnection Connection { get; set; }

        public Contracts.IMessage Supply()
        {
            ObjectMessage<DataTable> result = null;

            if (Connection == null)
            {
                RebuildConnection();
            }

            if (Connection != null)
            {
                using (SqlCommand cmd = new SqlCommand(SqlStatement, Connection))
                {
                    DataTable resultBody = new DataTable();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.HasRows)
                    {
                        resultBody.Load(reader);
                    }
                    result.Body = resultBody;
                }
            }

            return result;
        }

        private void RebuildConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Server;
            builder.InitialCatalog = Database;
            builder.IntegratedSecurity = true;

            Connection = new SqlConnection(builder.ConnectionString);
            Connection.Open();
        }

        public bool IsBlocking
        {
            get { return false; }
        }

        public string ReceviedFrom
        {
            get { return Server + "/" + Database + " (" +  Name + ")"; }
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(Server))
            {
                throw new ConfigurationException("Server is a required parameter for " + Name + " [" + this.GetType() + "]");
            }

            if (String.IsNullOrWhiteSpace(Database))
            {
                throw new ConfigurationException("Database is a required parameter for " + Name + " [" + this.GetType() + "]");
            }
        }
    }
}
