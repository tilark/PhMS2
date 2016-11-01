using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrasenLib;

namespace TranslationFromTrasen
{
    public class Main
    {
        public Main()
        {
            this.localConnection = "Server=192.168.100.162;Database=PhMs2;User Id=User_PhMs;Password=IkgnhzWEXpkyBghq;MultipleActiveResultSets=True;App=EntityFramework";
            this.trasenConnection = "data source=192.168.100.20;initial catalog=trasen;user id=public_user;password=hzhis;MultipleActiveResultSets=True;App=EntityFramework";
        }

        public Main(string local, string trasen)
        {
            this.localConnection = local;
            this.trasenConnection = trasen;
        }





        private string localConnection;

        private string trasenConnection;





        /// <summary>
        /// 获取病人与住院病例。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isRemoveCancel">指定是否将CANCEL_BIT为1的记录在本地删除。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        /// <remarks>时间点注意后边界。</remarks>
        /// <example>
        /// 获取2016年9月数据
        /// <code>
        /// GetInPatient(new DateTime(2016, 9, 1), new DateTime(2016, 10, 1));
        /// </code>
        /// </example>
        public void GetPatientAndInPatient(DateTime start, DateTime end, bool isRemoveCancel = false, bool isContainNullOutDate = true, bool isUpdateExists = true)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            //预取数据。
            var listPatients = db.Patients.ToList();
            var listInPatients = db.InPatients.ToList();

            //取VI_VINPATIENT。
            var queryTrasen_VI_VINPATIENTs = dbTrasen.VI_ZY_VINPATIENT.Where(c => (start <= c.OUT_DATE && c.OUT_DATE < end) || (start < c.CANCEL_DATE && c.CANCEL_DATE < end));
            if (isContainNullOutDate)
            {
                queryTrasen_VI_VINPATIENTs = queryTrasen_VI_VINPATIENTs.Union(dbTrasen.VI_ZY_VINPATIENT.Where(c => !c.OUT_DATE.HasValue));
            }
            var listTrasen_VI_VINPATIENT = queryTrasen_VI_VINPATIENTs.ToList();

            foreach (var itemVI_VINPATIENT in listTrasen_VI_VINPATIENT)
            {
                if (itemVI_VINPATIENT.CANCEL_BIT == 1)
                {
                    if (isRemoveCancel)
                    {
                        var inPatient = listInPatients.Where(c => c.Origin_INPATIENT_ID == itemVI_VINPATIENT.INPATIENT_ID).FirstOrDefault();

                        if (inPatient != null)
                        {
                            db.InPatients.Remove(inPatient);
                            db.SaveChanges();

                            listInPatients.Remove(inPatient);
                        }
                    }
                }
                else
                {
                    //处理Patient。
                    var patient = listPatients.Find(c => c.Origin_PATIENT_ID == itemVI_VINPATIENT.PATIENT_ID);
                    if (patient == null)
                    {
                        patient = new PhMS2dot1Domain.Models.Patient();

                        patient.PatientID = itemVI_VINPATIENT.PATIENT_ID;
                        patient.Origin_PATIENT_ID = itemVI_VINPATIENT.PATIENT_ID;
                        patient.BirthDate = itemVI_VINPATIENT.BIRTHDAY;

                        db.Patients.Add(patient);
                        db.SaveChanges();

                        listPatients.Add(patient);
                    }

                    //处理InPatient。
                    var InPatient = listInPatients.Find(c => c.Origin_INPATIENT_ID == itemVI_VINPATIENT.INPATIENT_ID);
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

                        listInPatients.Add(InPatient);
                    }
                    else
                    {
                        if (isUpdateExists)
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

        /// <summary>
        /// 获取用药记录。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        /// <remarks>时间点注意后边界。</remarks>
        public void GetDrugRecord(DateTime start, DateTime end, bool isContainNullOutDate = true, bool isUpdateExists = false)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            //预先获取整个表，方便取数。
            var listYP_YPCJD = dbTrasen.YP_YPCJD.ToList();
            var listYP_YPGGD = dbTrasen.YP_YPGGD.ToList();

            //取InPatients。
            var queryInPatients = db.InPatients.Where(c => start <= c.OutDate && c.OutDate < end);
            if (isContainNullOutDate)
                queryInPatients = queryInPatients.Union(db.InPatients.Where(c => !c.OutDate.HasValue));
            var listInPatient = queryInPatients.ToList();

            foreach (var itemInPatient in listInPatient)
            {
                //预先获取InPatient相关的InPatientDrugRecord，方便取数。
                var listInPatientDrugRecord = db.InPatientDrugRecords.Where(c => c.InPatientID == itemInPatient.InPatientID).ToList();

                //取对应的VI_ZY_ORDERRECORD。
                var listTrasen_VI_ZY_ORDERRECORD = dbTrasen.VI_ZY_ORDERRECORD.Where(c => c.INPATIENT_ID == itemInPatient.Origin_INPATIENT_ID && c.XMLY == 1).ToList();

                foreach (var itemVI_ZY_ORDERRECORD in listTrasen_VI_ZY_ORDERRECORD)
                {
                    if (itemVI_ZY_ORDERRECORD.HOITEM_ID == -1)
                        continue;

                    //关联的实例。
                    var objectYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemVI_ZY_ORDERRECORD.HOITEM_ID).First();
                    var objectYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == objectYP_YPCJD.GGID).First();

                    //获取是否已存在InPatientDrugRecord。
                    var inPatientDrugRecord = listInPatientDrugRecord.Where(c => c.Origin_ORDER_ID == itemVI_ZY_ORDERRECORD.ORDER_ID).FirstOrDefault();
                    if (inPatientDrugRecord == null)
                    {
                        inPatientDrugRecord = new PhMS2dot1Domain.Models.InPatientDrugRecord();

                        inPatientDrugRecord.InPatientDrugRecordID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                        inPatientDrugRecord.Origin_ORDER_ID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                        inPatientDrugRecord.InPatientID = itemVI_ZY_ORDERRECORD.INPATIENT_ID;
                        inPatientDrugRecord.Origin_KSSDJ = objectYP_YPGGD.KSSDJID;
                        inPatientDrugRecord.Origin_EXEC_DEPT = itemVI_ZY_ORDERRECORD.EXEC_DEPT;
                        inPatientDrugRecord.Origin_ORDER_DOC = itemVI_ZY_ORDERRECORD.ORDER_DOC;
                        inPatientDrugRecord.Origin_CJID = objectYP_YPCJD.CJID;
                        inPatientDrugRecord.ProductName = objectYP_YPCJD.S_YPPM;
                        inPatientDrugRecord.IsEssential = objectYP_YPGGD.GJJBYW.Value;
                        inPatientDrugRecord.DosageForm = objectYP_YPGGD.YPGG;
                        inPatientDrugRecord.DDD = objectYP_YPGGD.DDD.Value;
                        inPatientDrugRecord.Origin_ORDER_USAGE = itemVI_ZY_ORDERRECORD.ORDER_USAGE;

                        db.InPatientDrugRecords.Add(inPatientDrugRecord);
                    }
                    else
                    {
                        //已存在时，可以控制是否更新。
                        if (isUpdateExists)
                        {
                            inPatientDrugRecord.InPatientDrugRecordID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                            inPatientDrugRecord.Origin_ORDER_ID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                            inPatientDrugRecord.InPatientID = itemVI_ZY_ORDERRECORD.INPATIENT_ID;
                            inPatientDrugRecord.Origin_KSSDJ = objectYP_YPGGD.KSSDJID;
                            inPatientDrugRecord.Origin_EXEC_DEPT = itemVI_ZY_ORDERRECORD.EXEC_DEPT;
                            inPatientDrugRecord.Origin_ORDER_DOC = itemVI_ZY_ORDERRECORD.ORDER_DOC;
                            inPatientDrugRecord.Origin_CJID = objectYP_YPCJD.CJID;
                            inPatientDrugRecord.ProductName = objectYP_YPCJD.S_YPPM;
                            inPatientDrugRecord.IsEssential = objectYP_YPGGD.GJJBYW.Value;
                            inPatientDrugRecord.DosageForm = objectYP_YPGGD.YPGG;
                            inPatientDrugRecord.DDD = objectYP_YPGGD.DDD.Value;
                            inPatientDrugRecord.Origin_ORDER_USAGE = itemVI_ZY_ORDERRECORD.ORDER_USAGE;
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public void GetDrugFee(DateTime start, DateTime end, bool isContainNullOutDate = true, bool isUpdateExists = false)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            //取InPatients。
            var queryInPatients = db.InPatients.Where(c => start <= c.OutDate && c.OutDate < end);
            if (isContainNullOutDate)
                queryInPatients = queryInPatients.Union(db.InPatients.Where(c => !c.OutDate.HasValue));
            var listInPatient = queryInPatients.ToList();

            foreach (var itemInPatient in listInPatient)
            {
                //预先获取InPatient相关的DrugFee，方便取数。
                var listDrugFee = db.DrugFees.Where(c => c.InPatientDrugRecord.InPatientID == itemInPatient.InPatientID).ToList();

                //取对应的VI_ZY_FEE_SPECI。
                var listTrasen_VI_ZY_FEE_SPECI = dbTrasen.VI_ZY_FEE_SPECI.Where(c => c.INPATIENT_ID == itemInPatient.InPatientID && c.XMLY == 1).ToList();

                foreach (var itemVI_ZY_FEE_SPECI in listTrasen_VI_ZY_FEE_SPECI)
                {
                    if (itemVI_ZY_FEE_SPECI.ORDER_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                        continue;

                    var drugFee = listDrugFee.Where(c => c.Origin_ID == itemVI_ZY_FEE_SPECI.ID).FirstOrDefault();
                    if (drugFee == null)
                    {
                        drugFee = new PhMS2dot1Domain.Models.DrugFee();

                        drugFee.DrugFeeID = itemVI_ZY_FEE_SPECI.ID;
                        drugFee.Origin_ID = itemVI_ZY_FEE_SPECI.ID;
                        drugFee.UnitPrice = itemVI_ZY_FEE_SPECI.COST_PRICE;
                        drugFee.Origin_Unit = itemVI_ZY_FEE_SPECI.UNIT;
                        drugFee.Quantity = itemVI_ZY_FEE_SPECI.NUM;
                        drugFee.ActualPrice = itemVI_ZY_FEE_SPECI.ACVALUE;
                        drugFee.ChargeTime = itemVI_ZY_FEE_SPECI.CHARGE_DATE.Value;
                        drugFee.InPatientDrugRecordID = itemVI_ZY_FEE_SPECI.ORDER_ID;

                        db.DrugFees.Add(drugFee);
                    }
                    else
                    {
                        if (isUpdateExists)
                        {
                            drugFee.DrugFeeID = itemVI_ZY_FEE_SPECI.ID;
                            drugFee.Origin_ID = itemVI_ZY_FEE_SPECI.ID;
                            drugFee.UnitPrice = itemVI_ZY_FEE_SPECI.COST_PRICE;
                            drugFee.Origin_Unit = itemVI_ZY_FEE_SPECI.UNIT;
                            drugFee.Quantity = itemVI_ZY_FEE_SPECI.NUM;
                            drugFee.ActualPrice = itemVI_ZY_FEE_SPECI.ACVALUE;
                            drugFee.ChargeTime = itemVI_ZY_FEE_SPECI.CHARGE_DATE.Value;
                            drugFee.InPatientDrugRecordID = itemVI_ZY_FEE_SPECI.ORDER_ID;
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}