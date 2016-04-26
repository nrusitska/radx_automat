using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadXAutomat.Data
{
    public enum MedicationType { RadX, PureLive, RadAway}
    public class MedicationIntake
    {
        public virtual Int64 Id { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual MedicationType MedicationType { get; set; }
        public virtual int Amount { get; set; }
    }

    public class MedicationIntakeMap : ClassMap<MedicationIntake>
    {
        public MedicationIntakeMap()
        {            
            Id(x => x.Id).GeneratedBy.Increment();
            Version(x => x.Timestamp).Generated.Always();
            Map(x => x.MedicationType);
            Map(x => x.Amount);
        }
    }
}
