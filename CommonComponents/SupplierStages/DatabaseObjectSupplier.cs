using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.SupplierStages
{
    public abstract class DatabaseObjectSupplier<TObject> : ISupplierStage
        where TObject : new()
    {
        [ConfigurationParameter]
        public string Server { get; set; }

        [ConfigurationParameter]
        public string Database { get; set; }
        
        protected abstract string SqlStatement { get; }

        protected SqlConnection Connection { get; set; }

        public Contracts.IMessage Supply()
        {
            TObject
            if (Connection == null)
            {
                RebuildConnection();
            }

            if (Connection != null)
            {
                using (SqlCommand cmd = new SqlCommand(SqlStatement, Connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                }
            }
        }

        private void RebuildConnection()
        {
            throw new NotImplementedException();
        }

        public bool IsBlocking
        {
            get { return false; }
        }

        public string ReceviedFrom
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
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
