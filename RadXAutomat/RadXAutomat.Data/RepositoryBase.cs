using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public class RepositoryBase : IDisposable
    {
        protected ISession Session { get; set; }
        protected ITransaction UnitOfWork { get; set; }

        public RepositoryBase()
        {
            Session = DataManager.GetInstance().GetStaticSession();
            SetupNewUnitOfWork();
        }

        protected void SetupNewUnitOfWork()
        {
            if (UnitOfWork != null)
                UnitOfWork.Dispose();
            UnitOfWork = Session.BeginTransaction();
        }

        public void SaveOrUpdate(object obj)
        {
            Session.SaveOrUpdate(obj);
        }

        public void CommitOrRollbackOnError()
        {
            try
            {
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.Rollback();
            }
            finally
            {
                SetupNewUnitOfWork();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
