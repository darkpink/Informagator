using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public class DeleteEntityCommand<TEntity> : ICommand
        where TEntity : class
    {
        Func<ConfigurationEntities, DbSet<TEntity>> EntitySetGetter { get; set; }

        Func<DbSet<TEntity>, object, TEntity> EntityGetter { get; set; }

        Action<TEntity> AdditionalActionsBeforeDelete { get; set; }

        protected Action ActionAfterDelete { get; set; }

        public DeleteEntityCommand(Func<ConfigurationEntities, DbSet<TEntity>> entitySetGetter, Func<DbSet<TEntity>, object, TEntity> entityGetter,  Action<TEntity> additionalActionsBeforeDelete, Action actionAfterDelete)
        {
            EntitySetGetter = entitySetGetter;
            EntityGetter = entityGetter;
            AdditionalActionsBeforeDelete = additionalActionsBeforeDelete;
            ActionAfterDelete = actionAfterDelete;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                var entitySet = EntitySetGetter(entities);
                var entity = EntityGetter(entitySet, parameter);
                entitySet.Remove(entity);
                if (AdditionalActionsBeforeDelete != null)
                {
                    AdditionalActionsBeforeDelete(entity);
                }
                entities.SaveChanges();
            }

            if (ActionAfterDelete != null)
            {
                ActionAfterDelete();
            }
        }
    }
}
