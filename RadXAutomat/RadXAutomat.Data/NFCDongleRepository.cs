using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace RadXAutomat.Data
{
    public class NFCDongleRepository : RepositoryBase
    {
        public NFCDongle Get(string id)
        {
            return Session.Get<NFCDongle>(id);
        }
    }
   
}
