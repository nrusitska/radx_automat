using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public class DataManager
    {
        private static DataManager instance = new DataManager();
        public static DataManager GetInstance() { return instance; }

        private ISessionFactory _sf;
        public DataManager()
        {
                    
        }

        public void Init()
        {
            if(_sf == null)
                _sf = CreateConfiguration().BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            return _sf.OpenSession();
        }

        public FluentConfiguration CreateConfiguration()
        {
            var config = Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("radxautomat.db"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NFCDongleMap>());
            return config;
        }

        public void BuildDatabase()
        {
            CreateConfiguration().ExposeConfiguration(c => new SchemaExport(c).Create(true, true));
        }
    }
}
