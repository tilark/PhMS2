using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrasenLib;

namespace TranslationFromTrasen
{
    public static class Main
    {
        private const string LocalConnection = "Server=192.168.100.162;Database=PhMs2;User Id=User_PhMs;Password=IkgnhzWEXpkyBghq;MultipleActiveResultSets=True;App=EntityFramework";
        private const string TrasenConnection = "data source=192.168.100.20;initial catalog=trasen;user id=public_user;password=hzhis;MultipleActiveResultSets=True;App=EntityFramework";

        /// <summary>
        /// 获取住院病例数据。
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <remarks>时间点注意后边界。</remarks>
        /// <example>
        /// //获取2016年9月数据
        /// <code>
        /// GetInPatient(new DateTime(2016, 9, 1), new DateTime(2016, 10, 1));
        /// </code>
        /// </example>
        public static void GetInPatient(DateTime start, DateTime end)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(LocalConnection);
            var dbTrasen = new TrasenDbContext(TrasenConnection);

            var queryTrasen_VI_VINPATIENTs = dbTrasen.VI_ZY_VINPATIENT.Where(c => !c.OUT_DATE.HasValue || (start <= c.OUT_DATE && c.OUT_DATE < end) || (start < c.CANCEL_DATE && c.CANCEL_DATE < end));

            foreach (var itemVI_VINPATIENT in queryTrasen_VI_VINPATIENTs)
            {
                if (itemVI_VINPATIENT.CANCEL_BIT == 1)
                {

                }
                else
                {
                    var patient = db.Patients.Find(itemVI_VINPATIENT.PATIENT_ID);
                    if (patient == null)
                    {
                        patient = new PhMS2dot1Domain.Models.Patient();

                        patient.PatientID = itemVI_VINPATIENT.PATIENT_ID;
                        patient.Origin_PATIENT_ID = itemVI_VINPATIENT.PATIENT_ID;
                        patient.BirthDate = itemVI_VINPATIENT.BIRTHDAY;

                        db.Patients.Add(patient);
                        db.SaveChanges();
                    }

                    var InPatient = db.InPatients.Find(itemVI_VINPATIENT.INPATIENT_ID);
                    if (InPatient == null)
                    {
                        InPatient = new PhMS2dot1Domain.Models.InPatient();

                        InPatient.InPatientID = itemVI_VINPATIENT.INPATIENT_ID;
                        InPatient.Origin_INPATIENT_ID = itemVI_VINPATIENT.INPATIENT_ID;
                        InPatient.PatientID = patient.PatientID;
                        InPatient.CaseNumber = itemVI_VINPATIENT.INPATIENT_NO;
                        InPatient.Times = itemVI_VINPATIENT.TIMES;
                        InPatient.InDate = itemVI_VINPATIENT.IN_DATE.Value;
                        InPatient.Origin_IN_DEPT = itemVI_VINPATIENT.IN_DEPT;
                        InPatient.OutDate = itemVI_VINPATIENT.OUT_DATE;
                        InPatient.Origin_DEPT_ID = itemVI_VINPATIENT.DEPT_ID;

                        db.InPatients.Add(InPatient);
                        db.SaveChanges();
                    }
                    else
                    {
                        InPatient.Origin_INPATIENT_ID = itemVI_VINPATIENT.INPATIENT_ID;
                        InPatient.PatientID = patient.PatientID;
                        InPatient.CaseNumber = itemVI_VINPATIENT.INPATIENT_NO;
                        InPatient.Times = itemVI_VINPATIENT.TIMES;
                        InPatient.InDate = itemVI_VINPATIENT.IN_DATE.Value;
                        InPatient.Origin_IN_DEPT = itemVI_VINPATIENT.IN_DEPT;
                        InPatient.OutDate = itemVI_VINPATIENT.OUT_DATE;
                        InPatient.Origin_DEPT_ID = itemVI_VINPATIENT.DEPT_ID;

                        db.SaveChanges();
                    }
                }              
            }
        }
    }
}