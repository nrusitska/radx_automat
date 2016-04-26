using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
namespace RadXAutomat.Data
{
    public struct MedicationIntakeReport
    {
        public int RadX, PureLive, RadAway;
    }
    public class MedicationIntakeRepository : RepositoryBase
    {
        public IQueryable<MedicationIntake> Query()
        {
            return Session.Query<MedicationIntake>();
        }

        protected MedicationIntakeReport GetIntakeReport(DateTime time)
        {
            MedicationIntakeReport report = new MedicationIntakeReport();
            var timeQuery = Query().Where(i => i.Timestamp > time).ToList();
            report.RadX = timeQuery.Where(i => i.MedicationType == MedicationType.RadX).Sum(i => i.Amount);
            report.RadAway = timeQuery.Where(i => i.MedicationType == MedicationType.RadAway).Sum(i => i.Amount);
            report.PureLive = timeQuery.Where(i => i.MedicationType == MedicationType.PureLive).Sum(i => i.Amount);

            return report;
        }

        public void RecordIntake(MedicationType type, int amount = 1)
        {
            var intake = new MedicationIntake();
            intake.MedicationType = type;
            intake.Amount = amount;
            SaveOrUpdate(intake);
        }

        public void ClearStatistic()
        {
            var ini = new IniFile(DataManager.INI_PATH);
            string date = DateTime.Now.ToString();
            ini.IniWriteValue("Reports","ShowStatsSince", date);
        }

        protected DateTime GetLastReportTime()
        {
            var ini = new IniFile(DataManager.INI_PATH);
            string date = ini.IniReadValue("Reports", "ShowStatsSince");
            DateTime time = DateTime.MinValue;
            DateTime.TryParse(date, out time);
            return time;
        }

        public MedicationIntakeReport GetMedicationReport()
        {
            return GetIntakeReport(GetLastReportTime());
        }
    }
}
