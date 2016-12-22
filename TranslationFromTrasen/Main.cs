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
        #region 初始化

        public Main()
        {
            this.localConnection = "Server=192.168.100.162;Database=PhMs2;User Id=User_PhMs;Password=IkgnhzWEXpkyBghq;MultipleActiveResultSets=True;App=EntityFramework";
            this.trasenConnection = "data source=192.168.100.20;initial catalog=trasen;user id=public_user;password=hzhis;MultipleActiveResultSets=True;App=EntityFramework";

            this.MaxDegreeOfParallelism = defaultMaxDegreeOfParallelism;
        }

        public Main(int maxDegreeOfParallelism)
        {
            this.localConnection = "Server=192.168.100.162;Database=PhMs2;User Id=User_PhMs;Password=IkgnhzWEXpkyBghq;MultipleActiveResultSets=True;App=EntityFramework";
            this.trasenConnection = "data source=192.168.100.20;initial catalog=trasen;user id=public_user;password=hzhis;MultipleActiveResultSets=True;App=EntityFramework";

            this.MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        public Main(string local, string trasen)
        {
            this.localConnection = local;
            this.trasenConnection = trasen;

            this.MaxDegreeOfParallelism = defaultMaxDegreeOfParallelism;
        }

        public Main(string local, string trasen, int maxDegreeOfParallelism)
        {
            this.localConnection = local;
            this.trasenConnection = trasen;

            this.MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        #endregion





        #region 字段与属性

        private string localConnection;

        private string trasenConnection;

        private readonly int defaultMaxDegreeOfParallelism = 5;





        public int MaxDegreeOfParallelism { get; set; }

        #endregion





        /// <summary>
        /// 获取住院病例，并同时获取病人。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isRemoveCancel">指定是否将“CANCEL_BIT”为1的记录从本地删除。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExist">是否更新已存在记录。</param>
        /// <remarks>病人为住院、门诊共用。</remarks>
        /// <example>
        /// 获取2016年9月数据
        /// <code>
        /// GetPatienstAndInPatients(new DateTime(2016, 9, 1), new DateTime(2016, 10, 1));
        /// </code>
        /// </example>
        public void GetPatienstAndInPatients(DateTime start, DateTime end, bool isRemoveCancel = true, bool isContainNullOutDate = false, bool isUpdateExist = false)
        {
            //==初始化==

            #region 初始化

            var importDataLogPatient = new PhMS2dot1Domain.Models.ImportDataLog();
            var importDataLogInPatient = new PhMS2dot1Domain.Models.ImportDataLog();

            this.LogInitial(importDataLogPatient, "Trasen", "VI_ZY_VINPATIENT", "Patients", start, end);
            this.LogInitial(importDataLogInPatient, "Trasen", "VI_ZY_VINPATIENT", "InPatients", start, end);

            this.LogAppendRemarks(importDataLogPatient, string.Format("MethodName=GetPatienstAndInPatients. isRemoveCancel={0}, isContainNullOutDate={1}, IsUpdateExists={2}. ", isRemoveCancel, isContainNullOutDate, isUpdateExist));
            this.LogAppendRemarks(importDataLogInPatient, string.Format("MethodName=GetPatienstAndInPatients. isRemoveCancel={0}, isContainNullOutDate={1}, IsUpdateExists={2}. ", isRemoveCancel, isContainNullOutDate, isUpdateExist));

            #endregion

            //==对创新取数==

            #region 对创新取数

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_ZY_VINPATIENT = dbTrasen.VI_ZY_VINPATIENT.Where(c => (start <= c.OUT_DATE && c.OUT_DATE < end) || (start <= c.CANCEL_DATE && c.CANCEL_DATE < end));

            //（取创新中的原数据的可选筛选条件）
            if (isContainNullOutDate)
                queryTrasenVI_ZY_VINPATIENT = queryTrasenVI_ZY_VINPATIENT.Union(dbTrasen.VI_ZY_VINPATIENT.Where(c => !c.OUT_DATE.HasValue));

            this.LogSetReadStartTime(importDataLogPatient);
            this.LogSetReadStartTime(importDataLogInPatient);

            var listTrasen_VI_ZY_VINPATIENT = queryTrasenVI_ZY_VINPATIENT.ToList();
            var listTrasen_VI_ZY_VINPATIENT_Distinct = listTrasen_VI_ZY_VINPATIENT.Distinct(new Infrastructure.VI_ZY_VINPATIENT_Comparer()).ToList();

            this.LogSetReadEndTime(importDataLogPatient);
            this.LogSetReadEndTime(importDataLogInPatient);

            this.LogSetSourceRecordCount(importDataLogPatient, listTrasen_VI_ZY_VINPATIENT_Distinct.Count());
            this.LogSetSourceRecordCount(importDataLogInPatient, listTrasen_VI_ZY_VINPATIENT.Count());

            #endregion

            //==处理Patients==

            #region 处理Patients

            this.LogSetSuccessImportRecordCount(importDataLogPatient, (int)importDataLogPatient.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLogPatient);

            Parallel.ForEach(listTrasen_VI_ZY_VINPATIENT_Distinct, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_ZY_VINPATIENT, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var patient = dbParallel.Patients.Where(old => old.Origin_PATIENT_ID == itemTrasen_VI_ZY_VINPATIENT.PATIENT_ID).FirstOrDefault();
                if (patient == null)
                {
                    patient = new PhMS2dot1Domain.Models.Patient();

                    patient.PatientID = itemTrasen_VI_ZY_VINPATIENT.PATIENT_ID;
                    patient.Origin_PATIENT_ID = itemTrasen_VI_ZY_VINPATIENT.PATIENT_ID;
                    patient.BirthDate = itemTrasen_VI_ZY_VINPATIENT.BIRTHDAY;
                    patient.PatientName = itemTrasen_VI_ZY_VINPATIENT.NAME;

                    dbParallel.Patients.Add(patient);
                    dbParallel.SaveChanges();
                }
                else
                {
                    if (isUpdateExist)
                    {
                        patient.BirthDate = itemTrasen_VI_ZY_VINPATIENT.BIRTHDAY;
                        patient.PatientName = itemTrasen_VI_ZY_VINPATIENT.NAME;

                        dbParallel.SaveChanges();
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLogPatient);

            #endregion

            //==处理InPatients==

            #region 处理InPatients

            this.LogSetSuccessImportRecordCount(importDataLogInPatient, (int)importDataLogInPatient.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLogInPatient);

            Parallel.ForEach(listTrasen_VI_ZY_VINPATIENT, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_ZY_VINPATIENT, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
                var inPatient = dbParallel.InPatients.Where(c => c.Origin_INPATIENT_ID == itemTrasen_VI_ZY_VINPATIENT.INPATIENT_ID).FirstOrDefault();

                if (itemTrasen_VI_ZY_VINPATIENT.CANCEL_BIT != 0)
                {
                    if (isRemoveCancel)
                    {
                        if (inPatient != null)
                        {
                            dbParallel.InPatients.Remove(inPatient);
                        }
                    }
                }
                else
                {
                    if (inPatient == null)
                    {
                        inPatient = new PhMS2dot1Domain.Models.InPatient();

                        inPatient.InPatientID = itemTrasen_VI_ZY_VINPATIENT.INPATIENT_ID;
                        inPatient.PatientID = itemTrasen_VI_ZY_VINPATIENT.PATIENT_ID;
                        inPatient.CaseNumber = itemTrasen_VI_ZY_VINPATIENT.INPATIENT_NO;
                        inPatient.Times = itemTrasen_VI_ZY_VINPATIENT.TIMES;
                        inPatient.InDate = itemTrasen_VI_ZY_VINPATIENT.IN_DATE.Value;
                        inPatient.OutDate = itemTrasen_VI_ZY_VINPATIENT.OUT_DATE;
                        inPatient.Origin_INPATIENT_ID = itemTrasen_VI_ZY_VINPATIENT.INPATIENT_ID;
                        inPatient.Origin_IN_DEPT = itemTrasen_VI_ZY_VINPATIENT.IN_DEPT;
                        inPatient.Origin_DEPT_ID = itemTrasen_VI_ZY_VINPATIENT.DEPT_ID;

                        dbParallel.InPatients.Add(inPatient);
                        dbParallel.SaveChanges();
                    }
                    else
                    {
                        if (isUpdateExist)
                        {
                            inPatient.PatientID = itemTrasen_VI_ZY_VINPATIENT.PATIENT_ID;
                            inPatient.CaseNumber = itemTrasen_VI_ZY_VINPATIENT.INPATIENT_NO;
                            inPatient.Times = itemTrasen_VI_ZY_VINPATIENT.TIMES;
                            inPatient.InDate = itemTrasen_VI_ZY_VINPATIENT.IN_DATE.Value;
                            inPatient.OutDate = itemTrasen_VI_ZY_VINPATIENT.OUT_DATE;
                            inPatient.Origin_IN_DEPT = itemTrasen_VI_ZY_VINPATIENT.IN_DEPT;
                            inPatient.Origin_DEPT_ID = itemTrasen_VI_ZY_VINPATIENT.DEPT_ID;

                            dbParallel.SaveChanges();
                        }
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLogInPatient);

            #endregion

            //==完成==

            #region "完成"

            this.LogSave(importDataLogPatient);
            this.LogSave(importDataLogInPatient);

            Console.WriteLine("Finish Get GetPatienstAndInPatients：{0} To {1}.", start, end);

            #endregion
        }

        /// <summary>
        /// 获取住院用药记录。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        public void GetInPatientDrugRecords(DateTime start, DateTime end, bool isContainNullOutDate = false, bool isUpdateExists = false)
        {
            //==初始化==

            #region 初始化

            var importDataLog = new PhMS2dot1Domain.Models.ImportDataLog();

            this.LogInitial(importDataLog, "Trasen", "VI_ZY_ORDERRECORD, YP_YPCJD, YP_YPGGD", "InPatientDrugRecords", start, end);

            this.LogAppendRemarks(importDataLog, string.Format("MethodName=GetInPatientDrugRecords. isContainNullOutDate={0}. isUpdateExists={1}. （无法精确计算时间点）", isContainNullOutDate, isUpdateExists));

            #endregion

            this.LogSetReadStartTime(importDataLog);
            this.LogSetWriteStartTime(importDataLog);
            int sourceRecordCount = 0;

            var dbTrasen = new TrasenDbContext(this.trasenConnection);
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

            //预先获取整个表，方便取数。
            var listYP_YPCJD = dbTrasen.YP_YPCJD.ToList();
            var listYP_YPGGD = dbTrasen.YP_YPGGD.Where(c => !c.BDELETE).ToList();

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
                sourceRecordCount += listTrasen_VI_ZY_ORDERRECORD.Count();

                foreach (var itemVI_ZY_ORDERRECORD in listTrasen_VI_ZY_ORDERRECORD)
                {
                    //关联的实例。
                    var objectYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemVI_ZY_ORDERRECORD.HOITEM_ID).First();
                    var objectYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == objectYP_YPCJD.GGID).First();

                    //获取是否已存在InPatientDrugRecord。
                    var inPatientDrugRecord = listInPatientDrugRecord.Where(c => c.Origin_ORDER_ID.Value == itemVI_ZY_ORDERRECORD.ORDER_ID).FirstOrDefault();
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
                        inPatientDrugRecord.EffectiveConstituentAmount = objectYP_YPGGD.DDDJL.HasValue ? objectYP_YPGGD.DDDJL.Value : objectYP_YPGGD.HLXS;
                        inPatientDrugRecord.IsWesternMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 1;
                        inPatientDrugRecord.IsChinesePatentMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 2;
                        inPatientDrugRecord.IsTraditionalChineseMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 3;

                        db.InPatientDrugRecords.Add(inPatientDrugRecord);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (isUpdateExists)
                        {
                            inPatientDrugRecord.InPatientID = itemVI_ZY_ORDERRECORD.INPATIENT_ID;
                            inPatientDrugRecord.ProductName = objectYP_YPCJD.S_YPPM;
                            inPatientDrugRecord.IsEssential = objectYP_YPGGD.GJJBYW.Value;
                            inPatientDrugRecord.DosageForm = objectYP_YPGGD.YPGG;
                            inPatientDrugRecord.DDD = objectYP_YPGGD.DDD.Value;
                            inPatientDrugRecord.Origin_EXEC_DEPT = itemVI_ZY_ORDERRECORD.EXEC_DEPT;
                            inPatientDrugRecord.Origin_ORDER_DOC = itemVI_ZY_ORDERRECORD.ORDER_DOC;
                            inPatientDrugRecord.Origin_KSSDJID = objectYP_YPGGD.KSSDJID;
                            inPatientDrugRecord.Origin_CJID = objectYP_YPCJD.CJID;
                            inPatientDrugRecord.Origin_ORDER_USAGE = itemVI_ZY_ORDERRECORD.ORDER_USAGE;
                            inPatientDrugRecord.EffectiveConstituentAmount = objectYP_YPGGD.DDDJL.HasValue ? objectYP_YPGGD.DDDJL.Value : objectYP_YPGGD.HLXS;
                            inPatientDrugRecord.IsWesternMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 1;
                            inPatientDrugRecord.IsChinesePatentMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 2;
                            inPatientDrugRecord.IsTraditionalChineseMedicine = itemVI_ZY_ORDERRECORD.NTYPE == 3;

                            db.SaveChanges();
                        }
                    }
                }
            }

            this.LogSetReadEndTime(importDataLog);
            this.LogSetWriteEndTime(importDataLog);
            this.LogSetSourceRecordCount(importDataLog, sourceRecordCount);
            this.LogSetSuccessImportRecordCount(importDataLog, sourceRecordCount);
            this.LogSave(importDataLog);

            Console.WriteLine("Finish Get GetInPatientDrugRecords：{0} To {1}.", start, end);
        }

        /// <summary>
        /// 获取住院费用记录。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isRemoveDelete">指定是否将DELETE_BIT为1的记录在本地删除。</param>
        /// <param name="isContainNullOutDate">是否包含未出院记录。</param>
        /// <param name="isUpdateExists">是否更新已存在记录。</param>
        public void GetDrugFee(DateTime start, DateTime end, bool isRemoveDelete = true, bool isContainNullOutDate = false, bool isUpdateExists = true)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            var dbTrasen = new TrasenDbContext(this.trasenConnection);

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





        #region "门诊"

        /// <summary>
        /// 获取门诊病例，并同时获取病人。
        /// </summary>
        /// <param name="start">时段起点（闭区间）。</param>
        /// <param name="end">时段终点（开区间）。</param>
        /// <param name="isUpdateExist">是否更新已有记录。</param>
        /// <remarks>病人为住院、门诊共用。</remarks>
        public void GetPatientsAndOutPatients(DateTime start, DateTime end, bool isUpdateExist = false)
        {
            //==初始化==

            #region 初始化

            var importDataLogPatient = new PhMS2dot1Domain.Models.ImportDataLog();
            var importDataLogOutPatient = new PhMS2dot1Domain.Models.ImportDataLog();

            this.LogInitial(importDataLogPatient, "Trasen", "VI_MZ_GHXX", "Patients", start, end);
            this.LogInitial(importDataLogOutPatient, "Trasen", "VI_MZ_GHXX", "OutPatients", start, end);

            this.LogAppendRemarks(importDataLogPatient, string.Format("MethodName=GetPatientsAndOutPatients. IsUpdateExists={0}. ", isUpdateExist));
            this.LogAppendRemarks(importDataLogOutPatient, string.Format("MethodName=GetPatientsAndOutPatients. IsUpdateExists={0}. ", isUpdateExist));

            #endregion

            //==对创新取数==

            #region 对创新取数

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_GHXX = dbTrasen.VI_MZ_GHXX.Where(c => (start <= c.GHDJSJ && c.GHDJSJ < end) || (c.QXGHSJ.HasValue && start <= c.QXGHSJ && c.QXGHSJ < end));
            //（取创新中的原数据的可选筛选条件，暂无）

            this.LogSetReadStartTime(importDataLogPatient);
            this.LogSetReadStartTime(importDataLogOutPatient);

            var listTrasen_VI_MZ_GHXX = queryTrasenVI_MZ_GHXX.ToList();
            var listTrasen_VI_MZ_GHXX_Distinct = listTrasen_VI_MZ_GHXX.Distinct(new Infrastructure.VI_MZ_GHXX_Comparer()).ToList();

            this.LogSetReadEndTime(importDataLogPatient);
            this.LogSetReadEndTime(importDataLogOutPatient);

            this.LogSetSourceRecordCount(importDataLogPatient, listTrasen_VI_MZ_GHXX_Distinct.Count());
            this.LogSetSourceRecordCount(importDataLogOutPatient, listTrasen_VI_MZ_GHXX.Count());

            #endregion

            //==处理Patients==

            #region 处理Patients

            this.LogSetSuccessImportRecordCount(importDataLogPatient, (int)importDataLogPatient.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLogPatient);

            Parallel.ForEach(listTrasen_VI_MZ_GHXX_Distinct, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_MZ_GHXX, state, index) =>
            {
                //-读取时间未累积-
                var dbTrasenParallel = new TrasenDbContext(this.trasenConnection);
                var itemTrasen_VI_YY_BRXX = dbTrasenParallel.VI_YY_BRXX.Where(c => c.BRXXID == itemTrasen_VI_MZ_GHXX.BRXXID).First();

                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var patient = dbParallel.Patients.Where(c => c.Origin_PATIENT_ID == itemTrasen_VI_MZ_GHXX.BRXXID).FirstOrDefault();
                if (patient == null)
                {
                    patient = new PhMS2dot1Domain.Models.Patient();

                    patient.PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value;
                    patient.Origin_PATIENT_ID = itemTrasen_VI_MZ_GHXX.BRXXID.Value;
                    patient.PatientName = itemTrasen_VI_YY_BRXX.BRXM;
                    patient.BirthDate = itemTrasen_VI_YY_BRXX.CSRQ;

                    dbParallel.Patients.Add(patient);
                    dbParallel.SaveChanges();
                }
                else
                {
                    if (isUpdateExist)
                    {
                        patient.PatientName = itemTrasen_VI_YY_BRXX.BRXM;
                        patient.BirthDate = itemTrasen_VI_YY_BRXX.CSRQ;
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLogPatient);

            #endregion

            //==处理OutPatients==

            #region 处理OutPatients

            this.LogSetSuccessImportRecordCount(importDataLogOutPatient, (int)importDataLogOutPatient.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLogOutPatient);

            Parallel.ForEach(listTrasen_VI_MZ_GHXX, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasen_VI_MZ_GHXX, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatient = dbParallel.OutPatients.Where(c => c.Origin_GHXXID == itemTrasen_VI_MZ_GHXX.GHXXID).FirstOrDefault();
                if (outPatient == null)
                {
                    outPatient = new PhMS2dot1Domain.Models.OutPatient();

                    outPatient.OutPatientID = itemTrasen_VI_MZ_GHXX.GHXXID;
                    outPatient.PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value;
                    outPatient.Origin_GHXXID = itemTrasen_VI_MZ_GHXX.GHXXID;
                    outPatient.Origin_GHLB = itemTrasen_VI_MZ_GHXX.GHLB;
                    outPatient.ChargeTime = itemTrasen_VI_MZ_GHXX.GHDJSJ.Value;
                    outPatient.CancelChargeTime = itemTrasen_VI_MZ_GHXX.QXGHSJ;

                    dbParallel.OutPatients.Add(outPatient);
                    dbParallel.SaveChanges();
                }
                else
                {
                    if (isUpdateExist)
                    {
                        outPatient.Origin_GHLB = itemTrasen_VI_MZ_GHXX.GHLB;
                        outPatient.ChargeTime = itemTrasen_VI_MZ_GHXX.GHDJSJ.Value;
                        outPatient.CancelChargeTime = itemTrasen_VI_MZ_GHXX.QXGHSJ;
                        outPatient.PatientID = itemTrasen_VI_MZ_GHXX.BRXXID.Value;

                        dbParallel.SaveChanges();
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLogOutPatient);

            #endregion

            //==完成==

            #region "完成"

            this.LogSave(importDataLogPatient);
            this.LogSave(importDataLogOutPatient);

            Console.WriteLine("Finish Get GetPatientsAndOutPatients：{0} To {1}.", start, end);

            #endregion
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
            //==初始化==

            #region 初始化

            var importDataLog = new PhMS2dot1Domain.Models.ImportDataLog();

            this.LogInitial(importDataLog, "Trasen", "VI_MZ_CFB", "OutPatientPrescriptions", start, end);

            this.LogAppendRemarks(importDataLog, string.Format("MethodName=GetOutPatientPrescriptions. IsUpdateExists={0}. ", isUpdateExists));

            #endregion

            //==对创新取数==

            #region "对创新取数"

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_CFB = dbTrasen.VI_MZ_CFB.Where(c => start <= c.SFRQ && c.SFRQ < end);
            //（取创新中的原数据的可选筛选条件，暂无）

            this.LogSetReadStartTime(importDataLog);

            var listTrasenVI_MZ_CFB = queryTrasenVI_MZ_CFB.ToList();

            this.LogSetReadEndTime(importDataLog);
            this.LogSetSourceRecordCount(importDataLog, listTrasenVI_MZ_CFB.Count());

            #endregion

            //==处理OutPatientPrescriptions==

            #region "处理OutPatientPrescriptions"

            this.LogSetSuccessImportRecordCount(importDataLog, (int)importDataLog.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLog);

            Parallel.ForEach(listTrasenVI_MZ_CFB, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasenVI_MZ_CFB, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatientPrescription = dbParallel.OutPatientPrescriptions.Where(c => c.Origin_CFID == itemTrasenVI_MZ_CFB.CFID).FirstOrDefault();
                if (outPatientPrescription == null)
                {
                    //检测父节点
                    if (!dbParallel.OutPatients.Where(c => c.OutPatientID == itemTrasenVI_MZ_CFB.GHXXID.Value).Any())
                    {
                        this.LogAppendErrorMessage(importDataLog, "Failded Insert OutPatientPrescription: index:" + index + ", OutPatientPrescriptionID:" + itemTrasenVI_MZ_CFB.CFID + ". ");
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
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLog);

            #endregion

            //==完成==

            #region "完成"

            this.LogSave(importDataLog);

            Console.WriteLine("Finish Get OutPatientPrescriptions：{0} To {1}.", start, end);

            #endregion
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
            //==初始化==

            #region "初始化"

            var importDataLog = new PhMS2dot1Domain.Models.ImportDataLog();

            this.LogInitial(importDataLog, "Trasen", "VI_MZ_CFB_MX, YP_YPCJD, YP_YPGGD", "OutPatientDrugRecords", start, end);

            this.LogAppendRemarks(importDataLog, string.Format("MethodName=GetOutPatientDrugRecords. IsUpdateExists={0}. ", isUpdateExists));

            #endregion

            //==对创新取数==

            #region "对创新取数"

            var dbTrasen = new TrasenDbContext(this.trasenConnection);

            var queryTrasenVI_MZ_CFB_MX = dbTrasen.VI_MZ_CFB_MX.Where(c => start <= c.QRSJ && c.QRSJ < end && (c.TJDXMDM == "01" || c.TJDXMDM == "02" || c.TJDXMDM == "03"));
            //（取创新中的原数据的可选筛选条件，暂无）

            this.LogSetReadStartTime(importDataLog);

            var listTrasenVI_MZ_CFB_MX = queryTrasenVI_MZ_CFB_MX.ToList();

            var listYP_YPCJD = dbTrasen.YP_YPCJD.ToList();
            var listYP_YPGGD = dbTrasen.YP_YPGGD.Where(c => !c.BDELETE).ToList();

            this.LogSetReadEndTime(importDataLog);
            this.LogSetSourceRecordCount(importDataLog, listTrasenVI_MZ_CFB_MX.Count());

            #endregion

            //==处理OutPatientDrugRecords==。

            #region "处理OutPatientDrugRecords"

            this.LogSetSuccessImportRecordCount(importDataLog, (int)importDataLog.SourceRecordCount);
            this.LogSetWriteStartTime(importDataLog);

            Parallel.ForEach(listTrasenVI_MZ_CFB_MX, new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, (itemTrasenVI_MZ_CFB_MX, state, index) =>
            {
                var dbParallel = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);

                var outPatientDrugRecord = dbParallel.OutPatientDrugRecords.Where(c => c.Origin_CFMXID == itemTrasenVI_MZ_CFB_MX.CFMXID).FirstOrDefault();
                if (outPatientDrugRecord == null)
                {
                    //检测父节点
                    if (!dbParallel.OutPatientPrescriptions.Where(c => c.OutPatientPrescriptionID == itemTrasenVI_MZ_CFB_MX.CFID).Any())
                    {
                        this.LogAppendErrorMessage(importDataLog, "Failded Insert OutPatientDrugRecord: index:" + index + ", OutPatientDrugRecordID:" + itemTrasenVI_MZ_CFB_MX.CFMXID + ". ");
                    }
                    else
                    {
                        var itemYP_YPCJD = listYP_YPCJD.Where(c => c.CJID == itemTrasenVI_MZ_CFB_MX.XMID).First();
                        var itemYP_YPGGD = listYP_YPGGD.Where(c => c.GGID == itemYP_YPCJD.GGID).First();

                        outPatientDrugRecord = new PhMS2dot1Domain.Models.OutPatientDrugRecord();

                        outPatientDrugRecord.OutPatientDrugRecordID = itemTrasenVI_MZ_CFB_MX.CFMXID;
                        outPatientDrugRecord.OutPatientPrescriptionID = itemTrasenVI_MZ_CFB_MX.CFID;
                        outPatientDrugRecord.ProductName = itemTrasenVI_MZ_CFB_MX.PM;
                        outPatientDrugRecord.IsEssential = itemYP_YPGGD.GJJBYW.Value;
                        outPatientDrugRecord.Origin_CFMXID = itemTrasenVI_MZ_CFB_MX.CFMXID;
                        outPatientDrugRecord.Origin_KSSDJID = itemYP_YPGGD.KSSDJID;
                        outPatientDrugRecord.Origin_CJID = (int)itemTrasenVI_MZ_CFB_MX.XMID;
                        outPatientDrugRecord.IsWesternMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "01");
                        outPatientDrugRecord.IsChinesePatentMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "02");
                        outPatientDrugRecord.IsTraditionalChineseMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "03");
                        outPatientDrugRecord.DosageForm = itemTrasenVI_MZ_CFB_MX.GG;
                        outPatientDrugRecord.Ddd = itemYP_YPGGD.DDD.Value;
                        outPatientDrugRecord.Origin_YFMC = itemTrasenVI_MZ_CFB_MX.YFMC;
                        outPatientDrugRecord.UnitPrice = itemTrasenVI_MZ_CFB_MX.DJ;
                        outPatientDrugRecord.UnitName = itemTrasenVI_MZ_CFB_MX.DW;
                        outPatientDrugRecord.Quantity = itemTrasenVI_MZ_CFB_MX.SL;
                        outPatientDrugRecord.ActualPrice = itemTrasenVI_MZ_CFB_MX.JE;
                        outPatientDrugRecord.EffectiveConstituentAmount = itemYP_YPGGD.DDDJL.HasValue ? itemYP_YPGGD.DDDJL.Value : itemYP_YPGGD.HLXS;

                        dbParallel.OutPatientDrugRecords.Add(outPatientDrugRecord);
                        dbParallel.SaveChanges();
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
                        outPatientDrugRecord.IsTraditionalChineseMedicine = (itemTrasenVI_MZ_CFB_MX.TJDXMDM == "03");
                        outPatientDrugRecord.DosageForm = itemTrasenVI_MZ_CFB_MX.GG;
                        outPatientDrugRecord.Ddd = itemYP_YPGGD.DDD.Value;
                        outPatientDrugRecord.Origin_YFMC = itemTrasenVI_MZ_CFB_MX.YFMC;
                        outPatientDrugRecord.UnitPrice = itemTrasenVI_MZ_CFB_MX.DJ;
                        outPatientDrugRecord.UnitName = itemTrasenVI_MZ_CFB_MX.DW;
                        outPatientDrugRecord.Quantity = itemTrasenVI_MZ_CFB_MX.SL;
                        outPatientDrugRecord.ActualPrice = itemTrasenVI_MZ_CFB_MX.JE;
                        outPatientDrugRecord.EffectiveConstituentAmount = itemYP_YPGGD.DDDJL.HasValue ? itemYP_YPGGD.DDDJL.Value : itemYP_YPGGD.HLXS;

                        dbParallel.SaveChanges();
                    }
                }
            });

            this.LogSetWriteEndTime(importDataLog);

            #endregion

            //==完成==

            #region "完成"

            this.LogSave(importDataLog);

            Console.WriteLine("Finish Get OutPatientDrugRecords：{0} To {1}.", start, end);

            #endregion
        }

        #endregion







        private void LogInitial(PhMS2dot1Domain.Models.ImportDataLog importDataLog, string sourceDatabaseName, string sourceTableName, string localTableName, DateTime startTime, DateTime endTime)
        {
            importDataLog.SourceDatabaseName = sourceDatabaseName;
            importDataLog.SourceTableName = sourceTableName;
            importDataLog.LocalTableName = localTableName;
            importDataLog.StartTime = startTime;
            importDataLog.EndTime = endTime;
            importDataLog.Remarks = "";
            importDataLog.ErrorMessage = "";
        }

        private void LogSetSourceRecordCount(PhMS2dot1Domain.Models.ImportDataLog importDataLog, int count)
        {
            importDataLog.SourceRecordCount = count;
        }

        private void LogSetSuccessImportRecordCount(PhMS2dot1Domain.Models.ImportDataLog importDataLog, int count)
        {
            importDataLog.SuccessImportRecordCount = count;
        }

        private void LogSetReadStartTime(PhMS2dot1Domain.Models.ImportDataLog importDataLog, DateTime? time = null)
        {
            if (time == null)
                time = DateTime.Now;

            importDataLog.ReadStartTime = time.Value;
        }

        private void LogSetReadEndTime(PhMS2dot1Domain.Models.ImportDataLog importDataLog, DateTime? time = null)
        {
            if (time == null)
                time = DateTime.Now;

            importDataLog.ReadEndTime = time.Value;
        }

        private void LogSetWriteStartTime(PhMS2dot1Domain.Models.ImportDataLog importDataLog, DateTime? time = null)
        {
            if (time == null)
                time = DateTime.Now;

            importDataLog.WriteStartTime = time.Value;
        }

        private void LogSetWriteEndTime(PhMS2dot1Domain.Models.ImportDataLog importDataLog, DateTime? time = null)
        {
            if (time == null)
                time = DateTime.Now;

            importDataLog.WriteEndTime = time.Value;
        }

        private void LogSave(PhMS2dot1Domain.Models.ImportDataLog importDataLog)
        {
            var db = new PhMS2dot1Domain.Models.PhMS2dot1DomainContext(this.localConnection);
            db.ImportDataLogs.Add(importDataLog);
            db.SaveChanges();
        }

        private void LogAppendErrorMessage(PhMS2dot1Domain.Models.ImportDataLog importDataLog, string message)
        {
            lock (importDataLog)
            {
                importDataLog.ErrorMessage += message;
                importDataLog.SuccessImportRecordCount -= 1;
            }
        }

        private void LogAppendRemarks(PhMS2dot1Domain.Models.ImportDataLog importDataLog, string remarks)
        {
            importDataLog.Remarks += remarks;
        }
    }
}