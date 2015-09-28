using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public enum PersonState
    {
        Wastelander=0,
        Friend,
        Whitestar,
        Victor,
        JackBones
    }

    public class NFCDongle
    {
        public virtual string ID { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual PersonState Standing { get; set; }
        
    }

    public class NFCDongleMap : ClassMap<NFCDongle>
    {
        public NFCDongleMap()
        {
            Id(x => x.ID);
            Map(x => x.OwnerName);
            Map(x => x.Standing);
        }
    }
}
