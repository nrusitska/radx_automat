using NHibernate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public class RepositoryBase : IDisposable
    {
        protected ISession Session { get; set; }
        protected ITransaction UnitOfWork { get; set; }
        protected virtual ISession CreateSession()
        {
            return DataManager.GetInstance().OpenSession();
        }
        public RepositoryBase()
        {            
            SetupNewUnitOfWork();
        }

        protected void SetupNewUnitOfWork()
        {
            if (UnitOfWork != null)
                UnitOfWork.Dispose();
            if (Session != null)
                Session.Dispose();
            Session = CreateSession();
            Session.FlushMode = FlushMode.Commit;
            UnitOfWork = Session.BeginTransaction();
            UnitOfWork.Begin();
        }
        public void Save(object obj)
        {
            Session.Save(obj);
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
                Session.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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
