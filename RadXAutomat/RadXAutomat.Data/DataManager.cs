using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public class DataManager
    {
        public const string INI_PATH = "RadXAutomat.ini";
        public const string DB_PATH = "radxautomat.db";
        private static DataManager instance = new DataManager();
        public static DataManager GetInstance() { return instance; }

        private ISessionFactory _sf;
        private ISession _session;
        public DataManager()
        {
                    
        }

        public void Init()
        {
            if (_sf == null)
            {
                if (!File.Exists(DB_PATH))
                    BuildDatabase();
                _sf = CreateConfiguration().BuildSessionFactory();
            }
        }
        
        public ISession GetStaticSession()
        {
            if (_session == null)
                _session = OpenSession();
            return _session;
        }
        public ISession OpenSession()
        {
            return _sf.OpenSession();
        }

        public FluentConfiguration CreateConfiguration()
        {
            var drvType = typeof(System.Data.SQLite.SQLiteDataAdapter);
            var config = Fluently.Configure().Database(  SQLiteConfiguration.Standard.UsingFile(DB_PATH))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NFCDongleMap>());
            return config;
        }

        public void BuildDatabase()
        {
            var cfg = CreateConfiguration().ExposeConfiguration(c => new SchemaExport(c).Create(true, true));
            cfg.BuildConfiguration();
        }

        public void Close()
        {
            if(_sf != null)
            {
                _sf.Close();
                _sf.Dispose();
                _sf = null;
            }
        }
    }
}
