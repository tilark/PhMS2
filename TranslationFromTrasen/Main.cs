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

            MaxDegreeOfParallelism = 20;
        }

        public Main(string local, string trasen)
        {
            this.localConnection = local;
            this.trasenConnection = trasen;

            MaxDegreeOfParallelism = 20;
        }





        private string localConnection;

        private string trasenConnection;





        public int MaxDegreeOfParallelism { get; set; }





        /// <summary>
        /// 获取住院病例，并同时获取病人。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isRemoveCancel">指定是否将CANCEL_BIT为1的记录在本地删除。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        /// <remarks>病人为住院、门诊共用。</remarks>
        /// <example>
        /// 获取2016年9月数据
        /// <code>
        /// GetPatientAndInPatient(new DateTime(2016, 9, 1), new DateTime(2016, 10, 1));
        /// </code>
        /// </example>
        public void GetPatientAndInPatient(DateTime start, DateTime end, bool isRemoveCancel = false, bool isContainNullOutDate = true, bool isUpdateExists = true)
        {
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //处理Patients。

            var queryTrasenVI_ZY_VINPATIENT = dbTrasen.VI_ZY_VINPATIENT.Where(c => (start <= c.OUT_DATE && c.OUT_DATE < end) || (start <= c.CANCEL_DATE && c.CANCEL_DATE < end));
            if (isContainNullOutDate)
                queryTrasenVI_ZY_VINPATIENT = queryTrasenVI_ZY_VINPATIENT.Union(dbTrasen.VI_ZY_VINPATIENT.Where(c => !c.OUT_DATE.HasValue));
            var listTrasen_VI_ZY_VINPATIENT = queryTrasenVI_ZY_VINPATIENT.ToList();

            var listNewPatients = listTrasen_VI_ZY_VINPATIENT
                .Select(c => new PhMS2dot1Domain.Models.Patient
                {
                    PatientID = c.PATIENT_ID,
                    BirthDate = c.BIRTHDAY,
                    Origin_PATIENT_ID = c.PATIENT_ID
                }).Distinct(new PhMS2dot1Domain.Models.PatientComparer()).ToList();

            Parallel.ForEach(listNewPatients, (newPatient, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var oldPatient = dbParallel.Patients.Where(old => old.Origin_PATIENT_ID == newPatient.Origin_PATIENT_ID).FirstOrDefault();
                if (oldPatient == null)
                {
                    dbParallel.Patients.Add(newPatient);
                    dbParallel.SaveChanges();

                    Console.WriteLine("Insert Patient: index:" + index + ", PatientID:" + newPatient.PatientID);
                }
                else
                {
                    if (isUpdateExists)
                    {
                        oldPatient.BirthDate = newPatient.BirthDate;
                        dbParallel.SaveChanges();

                        Console.WriteLine("Update Patient: index:" + index + ", PatientID:" + newPatient.PatientID);
                    }
                }
            });

            return;

            //处理InPatients。

            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var listOldPatients = db.Patients.ToList();

            var listOldInPatients = db.InPatients.ToList();
            var listNewInPatients = dbTrasen.VI_ZY_VINPATIENT.Where(c => (start <= c.OUT_DATE && c.OUT_DATE < end) || (start <= c.CANCEL_DATE && c.CANCEL_DATE < end)).ToList()
                .Select(c => new PhMS2dot1Domain.Models.InPatient
                {
                    InPatientID = c.INPATIENT_ID,
                    PatientID = c.PATIENT_ID,
                    CaseNumber = c.INPATIENT_NO,
                    Times = c.TIMES,
                    InDate = c.IN_DATE.Value,
                    OutDate = c.OUT_DATE,
                    Origin_INPATIENT_ID = c.INPATIENT_ID,
                    Origin_IN_DEPT = c.IN_DEPT,
                    Origin_DEPT_ID = c.DEPT_ID,
                }).ToList();

            var listInPatientsToAdd = new List<PhMS2dot1Domain.Models.InPatient>();

            Parallel.ForEach(listNewInPatients, (c, state, index) =>
            {
                var oldInPatient = listOldInPatients.Where(old => old.Origin_INPATIENT_ID == c.Origin_INPATIENT_ID).FirstOrDefault();
                if (oldInPatient == null)
                {
                    listInPatientsToAdd.Add(new PhMS2dot1Domain.Models.InPatient
                    {
                        InPatientID = c.InPatientID,
                        PatientID = c.PatientID,
                        CaseNumber = c.CaseNumber,
                        Times = c.Times,
                        InDate = c.InDate,
                        OutDate = c.OutDate,
                        Origin_INPATIENT_ID = c.Origin_INPATIENT_ID,
                        Origin_IN_DEPT = c.Origin_IN_DEPT,
                        Origin_DEPT_ID = c.Origin_DEPT_ID,
                    });

                    //调试输出
                    Console.WriteLine("Insert InPatient: index:" + index + ", InPatientID:" + c.InPatientID);
                }
                else
                {
                    if (isUpdateExists)
                    {
                        oldInPatient.CaseNumber = c.CaseNumber;
                        oldInPatient.Times = c.Times;
                        oldInPatient.InDate = c.InDate;
                        oldInPatient.OutDate = c.OutDate;
                        oldInPatient.Origin_IN_DEPT = c.Origin_IN_DEPT;
                        oldInPatient.Origin_DEPT_ID = c.Origin_DEPT_ID;

                        //调试输出
                        Console.WriteLine("Update InPatient: index:" + index + ", InPatientID:" + c.InPatientID);
                    }
                }
            });

            db.InPatients.AddRange(listInPatientsToAdd);
            db.SaveChanges();

            return;

            //以下暂勿运行。









            var listInPatients = db.InPatients.ToList();

            //取VI_VINPATIENT。
            var queryTrasen_VI_ZY_VINPATIENTs = dbTrasen.VI_ZY_VINPATIENT.Where(c => (start <= c.OUT_DATE && c.OUT_DATE < end) || (start <= c.CANCEL_DATE && c.CANCEL_DATE < end));
            if (isContainNullOutDate)
                queryTrasen_VI_ZY_VINPATIENTs = queryTrasen_VI_ZY_VINPATIENTs.Union(dbTrasen.VI_ZY_VINPATIENT.Where(c => !c.OUT_DATE.HasValue));
            //var listTrasen_VI_ZY_VINPATIENT = queryTrasen_VI_ZY_VINPATIENTs.ToList();

            //foreach (var itemVI_ZY_VINPATIENT in listTrasen_VI_ZY_VINPATIENT)
            //{
            //    if (itemVI_ZY_VINPATIENT.CANCEL_BIT == 1)
            //    {
            //        if (isRemoveCancel)
            //        {
            //            var inPatient = listInPatients.Where(c => c.Origin_INPATIENT_ID == itemVI_ZY_VINPATIENT.INPATIENT_ID).FirstOrDefault();

            //            if (inPatient != null)
            //            {
            //                db.InPatients.Remove(inPatient);
            //                db.SaveChanges();

            //                listInPatients.Remove(inPatient);//可能是无效语句
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //处理Patient。
            //        var patient = listPatients.Find(c => c.Origin_PATIENT_ID == itemVI_ZY_VINPATIENT.PATIENT_ID);
            //        if (patient == null)
            //        {
            //            patient = new PhMS2dot1Domain.Models.Patient();

            //            patient.PatientID = itemVI_ZY_VINPATIENT.PATIENT_ID;
            //            patient.BirthDate = itemVI_ZY_VINPATIENT.BIRTHDAY;
            //            patient.Origin_PATIENT_ID = itemVI_ZY_VINPATIENT.PATIENT_ID;

            //            db.Patients.Add(patient);
            //            db.SaveChanges();

            //            listPatients.Add(patient);
            //        }

            //        //处理InPatient。
            //        var InPatient = listInPatients.Find(c => c.Origin_INPATIENT_ID == itemVI_ZY_VINPATIENT.INPATIENT_ID);
            //        if (InPatient == null)
            //        {
            //            InPatient = new PhMS2dot1Domain.Models.InPatient();

            //            InPatient.InPatientID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
            //            InPatient.PatientID = patient.PatientID;
            //            InPatient.CaseNumber = itemVI_ZY_VINPATIENT.INPATIENT_NO;
            //            InPatient.Times = itemVI_ZY_VINPATIENT.TIMES;
            //            InPatient.InDate = itemVI_ZY_VINPATIENT.IN_DATE.Value;
            //            InPatient.OutDate = itemVI_ZY_VINPATIENT.OUT_DATE;
            //            InPatient.Origin_INPATIENT_ID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
            //            InPatient.Origin_IN_DEPT = itemVI_ZY_VINPATIENT.IN_DEPT;
            //            InPatient.Origin_DEPT_ID = itemVI_ZY_VINPATIENT.DEPT_ID;

            //            db.InPatients.Add(InPatient);
            //            db.SaveChanges();

            //            listInPatients.Add(InPatient);
            //        }
            //        else
            //        {
            //            if (isUpdateExists)
            //            {
            //                InPatient.PatientID = patient.PatientID;
            //                InPatient.CaseNumber = itemVI_ZY_VINPATIENT.INPATIENT_NO;
            //                InPatient.Times = itemVI_ZY_VINPATIENT.TIMES;
            //                InPatient.InDate = itemVI_ZY_VINPATIENT.IN_DATE.Value;
            //                InPatient.OutDate = itemVI_ZY_VINPATIENT.OUT_DATE;
            //                InPatient.Origin_INPATIENT_ID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
            //                InPatient.Origin_IN_DEPT = itemVI_ZY_VINPATIENT.IN_DEPT;
            //                InPatient.Origin_DEPT_ID = itemVI_ZY_VINPATIENT.DEPT_ID;

            //                db.SaveChanges();
            //            }
            //        }
            //    }
            //}

            System.Threading.Tasks.Parallel.ForEach(listTrasen_VI_ZY_VINPATIENT, (itemVI_ZY_VINPATIENT) =>
            {
                if (itemVI_ZY_VINPATIENT.CANCEL_BIT == 1)
                {
                    if (isRemoveCancel)
                    {
                        var inPatient = listInPatients.Where(c => c.Origin_INPATIENT_ID == itemVI_ZY_VINPATIENT.INPATIENT_ID).FirstOrDefault();

                        if (inPatient != null)
                        {
                            db.InPatients.Remove(inPatient);
                            db.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    //处理Patient。
                    var patient = listOldPatients.Find(c => c.Origin_PATIENT_ID == itemVI_ZY_VINPATIENT.PATIENT_ID);
                    if (patient == null)
                    {
                        patient = new PhMS2dot1Domain.Models.Patient();

                        patient.PatientID = itemVI_ZY_VINPATIENT.PATIENT_ID;
                        patient.BirthDate = itemVI_ZY_VINPATIENT.BIRTHDAY;
                        patient.Origin_PATIENT_ID = itemVI_ZY_VINPATIENT.PATIENT_ID;

                        db.Patients.Add(patient);
                        db.SaveChangesAsync();

                        listOldPatients.Add(patient);
                    }

                    //处理InPatient。
                    var InPatient = listInPatients.Find(c => c.Origin_INPATIENT_ID == itemVI_ZY_VINPATIENT.INPATIENT_ID);
                    if (InPatient == null)
                    {
                        InPatient = new PhMS2dot1Domain.Models.InPatient();

                        InPatient.InPatientID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
                        InPatient.PatientID = patient.PatientID;
                        InPatient.CaseNumber = itemVI_ZY_VINPATIENT.INPATIENT_NO;
                        InPatient.Times = itemVI_ZY_VINPATIENT.TIMES;
                        InPatient.InDate = itemVI_ZY_VINPATIENT.IN_DATE.Value;
                        InPatient.OutDate = itemVI_ZY_VINPATIENT.OUT_DATE;
                        InPatient.Origin_INPATIENT_ID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
                        InPatient.Origin_IN_DEPT = itemVI_ZY_VINPATIENT.IN_DEPT;
                        InPatient.Origin_DEPT_ID = itemVI_ZY_VINPATIENT.DEPT_ID;

                        db.InPatients.Add(InPatient);
                        db.SaveChangesAsync();

                        listInPatients.Add(InPatient);
                    }
                    else
                    {
                        if (isUpdateExists)
                        {
                            InPatient.PatientID = patient.PatientID;
                            InPatient.CaseNumber = itemVI_ZY_VINPATIENT.INPATIENT_NO;
                            InPatient.Times = itemVI_ZY_VINPATIENT.TIMES;
                            InPatient.InDate = itemVI_ZY_VINPATIENT.IN_DATE.Value;
                            InPatient.OutDate = itemVI_ZY_VINPATIENT.OUT_DATE;
                            InPatient.Origin_INPATIENT_ID = itemVI_ZY_VINPATIENT.INPATIENT_ID;
                            InPatient.Origin_IN_DEPT = itemVI_ZY_VINPATIENT.IN_DEPT;
                            InPatient.Origin_DEPT_ID = itemVI_ZY_VINPATIENT.DEPT_ID;

                            db.SaveChangesAsync();
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 获取住院用药记录。
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
            var listInPatients = queryInPatients.ToList();

            foreach (var itemInPatient in listInPatients)
            {
                //预先获取InPatient相关的InPatientDrugRecord，方便取数。
                var listInPatientDrugRecord = itemInPatient.InPatientDrugRecords.ToList();

                //取对应的VI_ZY_ORDERRECORD。
                var listTrasen_VI_ZY_ORDERRECORD = dbTrasen.VI_ZY_ORDERRECORD.Where(c => c.INPATIENT_ID == itemInPatient.Origin_INPATIENT_ID && c.XMLY == 1 && c.HOITEM_ID != -1).ToList();

                foreach (var itemVI_ZY_ORDERRECORD in listTrasen_VI_ZY_ORDERRECORD)
                {
                    //关联的实例。
                    var objectYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemVI_ZY_ORDERRECORD.HOITEM_ID).First();
                    var objectYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == objectYP_YPCJD.GGID).First();

                    //获取是否已存在InPatientDrugRecord。
                    var inPatientDrugRecord = listInPatientDrugRecord.Where(c => c.Origin_ORDER_ID == itemVI_ZY_ORDERRECORD.ORDER_ID).FirstOrDefault();
                    if (inPatientDrugRecord == null)
                    {
                        inPatientDrugRecord = new PhMS2dot1Domain.Models.InPatientDrugRecord();

                        inPatientDrugRecord.InPatientDrugRecordID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                        inPatientDrugRecord.InPatientID = itemVI_ZY_ORDERRECORD.INPATIENT_ID;
                        inPatientDrugRecord.ProductName = objectYP_YPCJD.S_YPPM;
                        inPatientDrugRecord.IsEssential = objectYP_YPGGD.GJJBYW.Value;
                        inPatientDrugRecord.DosageForm = objectYP_YPGGD.YPGG;
                        inPatientDrugRecord.DDD = objectYP_YPGGD.DDD.Value;
                        inPatientDrugRecord.Origin_ORDER_ID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                        inPatientDrugRecord.Origin_EXEC_DEPT = itemVI_ZY_ORDERRECORD.EXEC_DEPT;
                        inPatientDrugRecord.Origin_ORDER_DOC = itemVI_ZY_ORDERRECORD.ORDER_DOC;
                        inPatientDrugRecord.Origin_KSSDJID = objectYP_YPGGD.KSSDJID;
                        inPatientDrugRecord.Origin_CJID = objectYP_YPCJD.CJID;
                        inPatientDrugRecord.Origin_ORDER_USAGE = itemVI_ZY_ORDERRECORD.ORDER_USAGE;
                        
                        db.InPatientDrugRecords.Add(inPatientDrugRecord);
                    }
                    else
                    {
                        //已存在时，可以控制是否更新。
                        if (isUpdateExists)
                        {
                            inPatientDrugRecord.InPatientID = itemVI_ZY_ORDERRECORD.INPATIENT_ID;
                            inPatientDrugRecord.ProductName = objectYP_YPCJD.S_YPPM;
                            inPatientDrugRecord.IsEssential = objectYP_YPGGD.GJJBYW.Value;
                            inPatientDrugRecord.DosageForm = objectYP_YPGGD.YPGG;
                            inPatientDrugRecord.DDD = objectYP_YPGGD.DDD.Value;
                            inPatientDrugRecord.Origin_ORDER_ID = itemVI_ZY_ORDERRECORD.ORDER_ID;
                            inPatientDrugRecord.Origin_EXEC_DEPT = itemVI_ZY_ORDERRECORD.EXEC_DEPT;
                            inPatientDrugRecord.Origin_ORDER_DOC = itemVI_ZY_ORDERRECORD.ORDER_DOC;
                            inPatientDrugRecord.Origin_KSSDJID = objectYP_YPGGD.KSSDJID;
                            inPatientDrugRecord.Origin_CJID = objectYP_YPCJD.CJID;
                            inPatientDrugRecord.Origin_ORDER_USAGE = itemVI_ZY_ORDERRECORD.ORDER_USAGE;
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取住院费用记录。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isRemoveDelete">指定是否将DELETE_BIT为1的记录在本地删除。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        public void GetDrugFee(DateTime start, DateTime end, bool isRemoveDelete = false, bool isContainNullOutDate = true, bool isUpdateExists = false)
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
                var listTrasen_VI_ZY_FEE_SPECI = dbTrasen.VI_ZY_FEE_SPECI.Where(c => c.INPATIENT_ID == itemInPatient.InPatientID && c.XMLY == 1 && c.ORDER_ID != new Guid("00000000-0000-0000-0000-000000000000")).ToList();

                foreach (var itemVI_ZY_FEE_SPECI in listTrasen_VI_ZY_FEE_SPECI)
                {
                    if (itemVI_ZY_FEE_SPECI.DELETE_BIT == 1)
                    {
                        if (isRemoveDelete)
                        {
                            var drugFee = db.DrugFees.Where(c => c.Origin_ID == itemVI_ZY_FEE_SPECI.ID).FirstOrDefault();

                            if (drugFee != null)
                            {
                                db.DrugFees.Remove(drugFee);
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var drugFee = listDrugFee.Where(c => c.Origin_ID == itemVI_ZY_FEE_SPECI.ID).FirstOrDefault();
                        if (drugFee == null)
                        {
                            drugFee = new PhMS2dot1Domain.Models.DrugFee();

                            drugFee.DrugFeeID = itemVI_ZY_FEE_SPECI.ID;
                            drugFee.InPatientDrugRecordID = itemVI_ZY_FEE_SPECI.ORDER_ID;
                            drugFee.UnitPrice = itemVI_ZY_FEE_SPECI.COST_PRICE;
                            drugFee.Quantity = itemVI_ZY_FEE_SPECI.NUM;
                            drugFee.ActualPrice = itemVI_ZY_FEE_SPECI.ACVALUE;
                            drugFee.ChargeTime = itemVI_ZY_FEE_SPECI.CHARGE_DATE.Value;
                            drugFee.Origin_ID = itemVI_ZY_FEE_SPECI.ID;
                            drugFee.Origin_Unit = itemVI_ZY_FEE_SPECI.UNIT;

                            db.DrugFees.Add(drugFee);
                        }
                        else
                        {
                            if (isUpdateExists)
                            {
                                drugFee.InPatientDrugRecordID = itemVI_ZY_FEE_SPECI.ORDER_ID;
                                drugFee.UnitPrice = itemVI_ZY_FEE_SPECI.COST_PRICE;
                                drugFee.Quantity = itemVI_ZY_FEE_SPECI.NUM;
                                drugFee.ActualPrice = itemVI_ZY_FEE_SPECI.ACVALUE;
                                drugFee.ChargeTime = itemVI_ZY_FEE_SPECI.CHARGE_DATE.Value;
                                drugFee.Origin_ID = itemVI_ZY_FEE_SPECI.ID;
                                drugFee.Origin_Unit = itemVI_ZY_FEE_SPECI.UNIT;
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public void GetDoctor(bool isUpdateExists = true)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            var listDoctors = db.Doctors.ToList();
            var listTrasen_JC_EMPLOYEE_PROPERTY = dbTrasen.JC_EMPLOYEE_PROPERTY.ToList();

            foreach (var itemTrasen_JC_EMPLOYEE_PROPERTY in listTrasen_JC_EMPLOYEE_PROPERTY)
            {
                var doctor = listDoctors.Where(c => c.ORIGIN_EMPLOYEE_ID == itemTrasen_JC_EMPLOYEE_PROPERTY.EMPLOYEE_ID).FirstOrDefault();

                if (doctor == null)
                {
                    doctor = new PhMS2dot1Domain.Models.Doctor();

                    doctor.DoctorName = itemTrasen_JC_EMPLOYEE_PROPERTY.NAME;
                    doctor.DoctorCode = itemTrasen_JC_EMPLOYEE_PROPERTY.D_CODE;
                    doctor.ORIGIN_EMPLOYEE_ID = itemTrasen_JC_EMPLOYEE_PROPERTY.EMPLOYEE_ID;

                    db.Doctors.Add(doctor);
                }
                else
                {
                    if (isUpdateExists)
                    {
                        doctor.DoctorName = itemTrasen_JC_EMPLOYEE_PROPERTY.NAME;
                        doctor.DoctorCode = itemTrasen_JC_EMPLOYEE_PROPERTY.D_CODE;
                        doctor.ORIGIN_EMPLOYEE_ID = itemTrasen_JC_EMPLOYEE_PROPERTY.EMPLOYEE_ID;
                    }
                }
                db.SaveChanges();
            }
        }

        public void GetDepartment(bool isUpdateExists = true)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            var listDepartments = db.Departments.ToList();
            var listTrasen_JC_DEPT_PROPERTY = dbTrasen.JC_DEPT_PROPERTY.ToList();

            foreach (var itemTrasen_JC_DEPT_PROPERTY in listTrasen_JC_DEPT_PROPERTY)
            {
                var department = listDepartments.Where(c => c.Origin_DEPT_ID == itemTrasen_JC_DEPT_PROPERTY.DEPT_ID).FirstOrDefault();

                if (department == null)
                {
                    department = new PhMS2dot1Domain.Models.Department();

                    department.DepartmentName = itemTrasen_JC_DEPT_PROPERTY.NAME;
                    department.Origin_DEPT_ID = itemTrasen_JC_DEPT_PROPERTY.DEPT_ID;

                    db.Departments.Add(department);
                }
                else
                {
                    if (isUpdateExists)
                    {
                        department.DepartmentName = itemTrasen_JC_DEPT_PROPERTY.NAME;
                        department.Origin_DEPT_ID = itemTrasen_JC_DEPT_PROPERTY.DEPT_ID;
                    }
                }
                db.SaveChanges();
            }
        }

        public void GetDrugUsage()
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            var Origin_ORDER_USAGEs = db.InPatientDrugRecords.Select(c => c.Origin_ORDER_USAGE).Distinct().ToList();
            var listDrugUsage = db.DrugUsages.ToList();

            foreach (var Origin_ORDER_USAGE in Origin_ORDER_USAGEs)
            {
                var drugUsage = listDrugUsage.Where(c => c.Origin_ORDER_USAGE == Origin_ORDER_USAGE).FirstOrDefault();

                if (drugUsage == null)
                {
                    drugUsage = new PhMS2dot1Domain.Models.DrugUsage();

                    drugUsage.IsUseForInjection = false;
                    drugUsage.IsUseForIntravenousTransfusion = false;
                    drugUsage.DrugUsageRemarks = "新增";
                    drugUsage.Origin_ORDER_USAGE = Origin_ORDER_USAGE;

                    db.DrugUsages.Add(drugUsage);
                }
                db.SaveChanges();
            }
        }

        public void GetAntiBioticLevel()
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            var Origin_KSSDJs = db.InPatientDrugRecords.Select(c => c.Origin_KSSDJID).Distinct().ToList();
            var listAntiBioticLevel = db.AntibioticLevels.ToList();

            foreach (var Origin_KSSDJ in Origin_KSSDJs)
            {
                var antiBioticLevel = listAntiBioticLevel.Where(c => c.Origin_KSSDJID == Origin_KSSDJ).FirstOrDefault();

                if (antiBioticLevel == null)
                {
                    antiBioticLevel = new PhMS2dot1Domain.Models.AntibioticLevel();

                    antiBioticLevel.IsAntibiotic = false;
                    antiBioticLevel.IsNonRestrict = false;
                    antiBioticLevel.IsRestrict = false;
                    antiBioticLevel.AntibioticLevelName = string.Empty;
                    antiBioticLevel.AntibioticLevelRemarks = "新增";
                    antiBioticLevel.Origin_KSSDJID = Origin_KSSDJ;

                    db.AntibioticLevels.Add(antiBioticLevel);
                }
                db.SaveChanges();
            }
        }

        public void GetUnit()
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

            //测试使用。
            {
                //db.Database.Log = Console.WriteLine;
                //dbTrasen.Database.Log = Console.WriteLine;
            }

            var Origin_UNITs = db.DrugFees.Select(c => c.Origin_Unit).Distinct().ToList();
            var listDrugUnit = db.DrugUnits.ToList();

            foreach (var Origin_UNIT in Origin_UNITs)
            {
                var drugUnit = listDrugUnit.Where(c => c.Origin_UNIT == Origin_UNIT).FirstOrDefault();

                if (drugUnit == null)
                {
                    drugUnit = new PhMS2dot1Domain.Models.DrugUnit();

                    drugUnit.IsUseByBottle = false;
                    drugUnit.Origin_UNIT = Origin_UNIT;

                    db.DrugUnits.Add(drugUnit);
                }
                db.SaveChanges();
            }
        }





        /// <summary>
        /// 获取门诊病例，并同时获取病人。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="IsUpdateExists">是否更新已有记录。</param>
        /// <remarks>病人为住院、门诊共用。</remarks>
        public void GetPatientsAndOutPatients(DateTime start, DateTime end, bool IsUpdateExists = false)
        {
            //==对创新取数==

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_GHXX = dbTrasen.VI_MZ_GHXX.Where(c => (start <= c.GHDJSJ && c.GHDJSJ < end) || (c.QXGHSJ.HasValue && start <= c.QXGHSJ && c.QXGHSJ < end));
            //（取创新中的原数据的可选筛选条件，暂无）
            var listTrasen_VI_MZ_GHXX = queryTrasenVI_MZ_GHXX.ToList();
            var listTrasen_VI_MZ_GHXX_Distinct = listTrasen_VI_MZ_GHXX.Distinct(new Infrastructure.VI_MZ_GHXX_Comparer()).ToList();

            //==处理Patients==

            Parallel.ForEach(listTrasen_VI_MZ_GHXX_Distinct, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_MZ_GHXX, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var patient = dbParallel.Patients.Where(c => c.Origin_PATIENT_ID == itemTrasen_VI_MZ_GHXX.BRXXID).FirstOrDefault();
                if (patient == null)
                {
                    patient = new PhMS2dot1Domain.Models.Patient
                    {
                        PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value,
                        Origin_PATIENT_ID = itemTrasen_VI_MZ_GHXX.BRXXID.Value,
                    };

                    dbParallel.Patients.Add(patient);
                    dbParallel.SaveChanges();

                    //Console.WriteLine("Insert Patient: index:" + index /*+ ", PatientID:" + patient.PatientID*/);
                }
                else
                {
                    if (IsUpdateExists)
                    {
                        //什么也不做（因为没有出生日期）

                        //Console.WriteLine("Update Patient: index:" + index /*+ ", PatientID:" + patient.PatientID*/);
                    }
                }
            });

            //==处理OutPatients==

            Parallel.ForEach(listTrasen_VI_MZ_GHXX, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_MZ_GHXX, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatient = dbParallel.OutPatients.Where(old => old.Origin_GHXXID == itemTrasen_VI_MZ_GHXX.GHXXID).FirstOrDefault();
                if (outPatient == null)
                {
                    outPatient = new PhMS2dot1Domain.Models.OutPatient
                    {
                        OutPatientID = itemTrasen_VI_MZ_GHXX.GHXXID,
                        PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value,
                        Origin_GHXXID = itemTrasen_VI_MZ_GHXX.GHXXID,
                        Origin_GHLB = itemTrasen_VI_MZ_GHXX.GHLB,
                        ChargeTime = itemTrasen_VI_MZ_GHXX.GHDJSJ.Value,
                        CancelChargeTime = itemTrasen_VI_MZ_GHXX.QXGHSJ,
                    };

                    dbParallel.OutPatients.Add(outPatient);
                    dbParallel.SaveChanges();

                    //Console.WriteLine("Insert OutPatient: index:" + index /*+ ", OutPatientID:" + outPatient.OutPatientID*/);
                }
                else
                {
                    if (IsUpdateExists)
                    {
                        outPatient.Origin_GHLB = itemTrasen_VI_MZ_GHXX.GHLB;
                        outPatient.ChargeTime = itemTrasen_VI_MZ_GHXX.GHDJSJ.Value;
                        outPatient.CancelChargeTime = itemTrasen_VI_MZ_GHXX.QXGHSJ;
                        outPatient.PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value;

                        dbParallel.SaveChanges();

                        //Console.WriteLine("Update OutPatient: index:" + index /*+ ", OutPatientID:" + outPatient.OutPatientID*/);
                    }
                }
            });

            //==完成==

            Console.WriteLine("Finish Get OutPatients：{0} To {1}.", start, end);
        }

        /// <summary>
        /// 获取门诊处方。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isUpdateExists">是否更新已有记录。</param>
        /// <remarks>存在部分“创新”中的处方表记录无法对应挂号记录的记录，目前以忽略处理。</remarks>
        public void GetOutPatientPrescriptions(DateTime start, DateTime end, bool isUpdateExists = false)
        {
            //==对创新取数==

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_CFB = dbTrasen.VI_MZ_CFB.Where(c => start <= c.SFRQ && c.SFRQ < end);
            //（取创新中的原数据的可选筛选条件，暂无）
            var listTrasenVI_MZ_CFB = queryTrasenVI_MZ_CFB.ToList();

            //==处理OutPatientPrescriptions==

            Parallel.ForEach(listTrasenVI_MZ_CFB, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasenVI_MZ_CFB, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatientPrescription = dbParallel.OutPatientPrescriptions.Where(c => c.Origin_CFID == itemTrasenVI_MZ_CFB.CFID).FirstOrDefault();
                if (outPatientPrescription == null)
                {
                    //检测父节点
                    if (!dbParallel.OutPatients.Where(c => c.OutPatientID == itemTrasenVI_MZ_CFB.GHXXID.Value).Any())
                    {
                        Console.WriteLine("Failded Insert OutPatientPrescription: index:" + index + ", OutPatientPrescriptionID:" + itemTrasenVI_MZ_CFB.CFID);
                    }
                    else
                    {
                        outPatientPrescription = new PhMS2dot1Domain.Models.OutPatientPrescription();

                        outPatientPrescription.OutPatientPrescriptionID = itemTrasenVI_MZ_CFB.CFID;
                        outPatientPrescription.OutPatientID = itemTrasenVI_MZ_CFB.GHXXID.Value;
                        outPatientPrescription.Origin_CFID = itemTrasenVI_MZ_CFB.CFID;
                        outPatientPrescription.ChargeTime = itemTrasenVI_MZ_CFB.SFRQ.Value;
                        outPatientPrescription.Origin_KSDM = itemTrasenVI_MZ_CFB.KSDM;
                        outPatientPrescription.Origin_YSDM = itemTrasenVI_MZ_CFB.YSDM;

                        dbParallel.OutPatientPrescriptions.Add(outPatientPrescription);
                        dbParallel.SaveChanges();

                        //Console.WriteLine("Insert OutPatientPrescription: index:" + index + ", OutPatientPrescriptionID:" + outPatientPrescription.OutPatientPrescriptionID);
                    }
                }
                else
                {
                    if (isUpdateExists)
                    {
                        outPatientPrescription.OutPatientID = itemTrasenVI_MZ_CFB.GHXXID.Value;
                        outPatientPrescription.ChargeTime = itemTrasenVI_MZ_CFB.SFRQ.Value;
                        outPatientPrescription.Origin_KSDM = itemTrasenVI_MZ_CFB.KSDM;
                        outPatientPrescription.Origin_YSDM = itemTrasenVI_MZ_CFB.YSDM;

                        dbParallel.SaveChanges();

                        //Console.WriteLine("Update OutPatientPrescription: index:" + index + ", OutPatientPrescriptionID:" + outPatientPrescription.OutPatientPrescriptionID);
                    }
                }
            });

            Console.WriteLine("Finish Get OutPatientPrescriptions：{0} To {1}.", start, end);
        }

        /// <summary>
        /// 获取门诊用药记录。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isUpdateExists">是否更新已有记录。</param>
        /// <remarks>存在部分“创新”中的处方明细表记录无法对应处方的记录，目前以忽略处理。</remarks>
        public void GetOutPatientDrugRecords(DateTime start, DateTime end, bool isUpdateExists = false)
        {
            //==对创新取数==

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_CFB_MX = dbTrasen.VI_MZ_CFB_MX.Where(c => start <= c.QRSJ && c.QRSJ < end && (c.TJDXMDM == "01" || c.TJDXMDM == "02" || c.TJDXMDM == "03"));
            //（取创新中的原数据的可选筛选条件，暂无）
            var listTrasenVI_MZ_CFB_MX = queryTrasenVI_MZ_CFB_MX.ToList();

            var listYP_YPCJD = dbTrasen.YP_YPCJD.Where(c => !c.BDELETE).ToList();
            var listYP_YPGGD = dbTrasen.YP_YPGGD.Where(c => !c.BDELETE).ToList();

            //==处理OutPatientDrugRecords==。

            Parallel.ForEach(listTrasenVI_MZ_CFB_MX, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasenVI_MZ_CFB_MX, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatientDrugRecord = dbParallel.OutPatientDrugRecords.Where(c => c.Origin_CFMXID == itemTrasenVI_MZ_CFB_MX.CFMXID).FirstOrDefault();
                if (outPatientDrugRecord == null)
                {
                    if (dbParallel.OutPatientPrescriptions.Where(c => c.OutPatientPrescriptionID == itemTrasenVI_MZ_CFB_MX.CFID).Any())
                    {

                        OutPatientDrugRecordID = itemTrasenVI_MZ_CFB_MX.CFMXID,
                        OutPatientPrescriptionID = itemTrasenVI_MZ_CFB_MX.CFID,
                        ProductName = itemTrasenVI_MZ_CFB_MX.PM,
                        IsEssential = itemYP_YPGGD.GJJBYW.Value,
                        Origin_CFMXID = itemTrasenVI_MZ_CFB_MX.CFMXID,
                        Origin_KSSDJID = itemYP_YPGGD.KSSDJID,
                        Origin_CJID = (int)itemTrasenVI_MZ_CFB_MX.XMID,
                        IsWesternMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "1"),
                        IsChinesePatentMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "2"),
                        DosageForm = itemTrasenVI_MZ_CFB_MX.GG,
                        Ddd = itemYP_YPGGD.DDD.Value,
                        Origin_YFMC = itemTrasenVI_MZ_CFB_MX.YFMC,
                        UnitPrice = itemTrasenVI_MZ_CFB_MX.DJ,
                        UnitName = itemTrasenVI_MZ_CFB_MX.DW,
                        Quantity = itemTrasenVI_MZ_CFB_MX.SL,
                        ActualPrice = itemTrasenVI_MZ_CFB_MX.JE,
                    };

                        var itemYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemTrasenVI_MZ_CFB_MX.XMID).First();
                        var itemYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == itemYP_YPCJD.GGID).First();


                        outPatientDrugRecord = new PhMS2dot1Domain.Models.OutPatientDrugRecord
                        {
                            OutPatientDrugRecordID = itemTrasenVI_MZ_CFB_MX.CFMXID,
                            OutPatientPrescriptionID = itemTrasenVI_MZ_CFB_MX.CFID,
                            ProductName = itemTrasenVI_MZ_CFB_MX.PM,
                            IsEssential = itemYP_YPGGD.GJJBYW.Value,
                            Origin_CFMXID = itemTrasenVI_MZ_CFB_MX.CFMXID,
                            Origin_KSSDJ = itemYP_YPGGD.KSSDJID,
                            Origin_CJID = (int)itemTrasenVI_MZ_CFB_MX.XMID,
                            IsWesternMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "01"),
                            IsChinesePatentMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "02"),
                            DosageForm = itemTrasenVI_MZ_CFB_MX.GG,
                            Ddd = itemYP_YPGGD.DDD.Value,
                            Origin_YFMC = itemTrasenVI_MZ_CFB_MX.YFMC,
                            UnitPrice = itemTrasenVI_MZ_CFB_MX.DJ,
                            UnitName = itemTrasenVI_MZ_CFB_MX.DW,
                            Quantity = itemTrasenVI_MZ_CFB_MX.SL,
                            ActualPrice = itemTrasenVI_MZ_CFB_MX.JE,
                        };

                        dbParallel.OutPatientDrugRecords.Add(outPatientDrugRecord);
                        dbParallel.SaveChanges();

                        //Console.WriteLine("Insert OutPatientDrugRecord: index:" + index + ", OutPatientDrugRecordID:" + outPatientDrugRecord.OutPatientDrugRecordID);
                    }
                    else
                    {
                        Console.WriteLine("Failded Insert OutPatientDrugRecord: index:" + index + ", OutPatientDrugRecordID:" + itemTrasenVI_MZ_CFB_MX.CFMXID);
                    }
                }
                else
                {
                    if (isUpdateExists)
                    {
                        var itemYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemTrasenVI_MZ_CFB_MX.XMID).First();
                        var itemYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == itemYP_YPCJD.GGID).First();

                        outPatientDrugRecord.OutPatientPrescriptionID = itemTrasenVI_MZ_CFB_MX.CFID;
                        outPatientDrugRecord.ProductName = itemTrasenVI_MZ_CFB_MX.PM;
                        outPatientDrugRecord.IsEssential = itemYP_YPGGD.GJJBYW.Value;
                        outPatientDrugRecord.Origin_KSSDJID = itemYP_YPGGD.KSSDJID;
                        outPatientDrugRecord.Origin_CJID = (int)itemTrasenVI_MZ_CFB_MX.XMID;
                        outPatientDrugRecord.IsWesternMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "01");
                        outPatientDrugRecord.IsChinesePatentMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "02");
                        outPatientDrugRecord.DosageForm = itemTrasenVI_MZ_CFB_MX.GG;
                        outPatientDrugRecord.Ddd = itemYP_YPGGD.DDD.Value;
                        outPatientDrugRecord.Origin_YFMC = itemTrasenVI_MZ_CFB_MX.YFMC;
                        outPatientDrugRecord.UnitPrice = itemTrasenVI_MZ_CFB_MX.DJ;
                        outPatientDrugRecord.UnitName = itemTrasenVI_MZ_CFB_MX.DW;
                        outPatientDrugRecord.Quantity = itemTrasenVI_MZ_CFB_MX.SL;
                        outPatientDrugRecord.ActualPrice = itemTrasenVI_MZ_CFB_MX.JE;

                        dbParallel.SaveChanges();

                        //Console.WriteLine("Update OutPatientDrugRecord: index:" + index + ", OutPatientDrugRecordID:" + outPatientDrugRecord.OutPatientDrugRecordID);
                    }
                }
            });

            Console.WriteLine("Finish Get OutPatientDrugRecords：{0} To {1}.", start, end);
        }
    }
}