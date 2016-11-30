using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.ViewModels;
using PhMS2dot1Domain.Models;
using System.Data.Entity;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    /// <summary>
    /// ImDepartmentAntibioticUsageRateDomain.获取各科室抗菌药物使用率集合
    /// </summary>
    public class ImDepartmentAntibioticUsageRateDomain : IDepartmentAntibioticUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;

        public ImDepartmentAntibioticUsageRateDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
            this.innerRepository = new InnerRepository(this.innerFactory);
        }
        public List<DepartmentAntibioticUsageRateDomain> GetDepartmentAntibioticUsageRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticUsageRateDomain>();
            try
            {


                //获取同期出院人数
                //将时间分为几个片段再取数，整合后再存到result中。
                var duration = endTime.Subtract(startTime).Days;
                var durationDay = 3;
                int maxDays = (int)duration / durationDay + 1;
                //var startTime2 = startTime;
                //var endTime2 = startTime2.AddDays(durationDay);
                var departmentPersonTotoalList = new List<OutDepartmentPerson>();
                var inPatientFromDrugRecordTotalList = new List<InPatient>();
                var antibioticPersonTotalList = new List<AntibioticPerson>();
                //同期出院人数
                departmentPersonTotoalList = innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime, endTime);

                //获取科室集合

                var departments = this.innerFactory.CreateDepartment().GetDepartment();

                //抗菌药物使用人的出院病人集合
                var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);
                //var inPatientList = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime);
                this.innerFactory.Dispose();
                var inPatientViews = inPatientFromDrugRecordList.Select(a => new InPatientView { InPatientID = a.InPatientID, CaseNumber = a.CaseNumber, InDate = a.InDate, Origin_DEPT_ID = a.Origin_DEPT_ID, OutDate = a.OutDate, Origin_IN_DEPT = a.Origin_IN_DEPT }).AsParallel().ToList();
                var inPatientViewList = new List<InPatientView>();
                inPatientViewList.Capacity = 2000;
                Parallel.ForEach(inPatientViews, (inPatient, state, index) =>
                {
                    //var drugRecordsList = new List<InPatientDrugRecord>();
                    var drugRecordViewsList = new List<InPatientDrugRecordView>();
                    drugRecordViewsList.Capacity = 1000;
                    inPatient.InPatientDrugRecordViews = new List<InPatientDrugRecordView>();
                    PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();

                    //var innerFactory = new Domain2dot1InnerFactory(context);                    
                    var drugRecords = context.InPatientDrugRecords.Where(a => a.InPatientID == inPatient.InPatientID).Select(b => new InPatientDrugRecordView { InPatientDrugRecordID = b.InPatientDrugRecordID, DDD = b.DDD, Origin_KSSDJ = b.Origin_KSSDJ });
                    //var drugRecords = context.InPatientDrugRecords.Where(a => a.InPatientID == inPatient.InPatientID);
                    //var drugFeesList = new List<DrugFee>();
                    Parallel.ForEach(drugRecords, (drugRecord, state2, index2) =>
                    {
                        drugRecord.DrugFeeViews = new List<DrugFeeView>();
                        PhMS2dot1DomainContext context2 = new PhMS2dot1DomainContext();
                        var drugFees = context2.DrugFees.Where(a => a.InPatientDrugRecordID == drugRecord.InPatientDrugRecordID).Select(b => new DrugFeeView { ActualPrice = b.ActualPrice, ChargeTime = b.ChargeTime }).ToList();
                        //var drugFees = context2.DrugFees.Where(a => a.InPatientDrugRecordID == drugRecord.InPatientDrugRecordID).ToList();
                        context2.Dispose();

                        if (drugFees != null)
                        {
                            //drugFeesList.AddRange(drugFees);
                            drugRecord.DrugFeeViews.AddRange(drugFees);
                            if (drugRecordViewsList.Capacity - drugRecordViewsList.Count < drugRecord.DrugFeeViews.Count)
                            {
                                drugRecordViewsList.Capacity += drugRecord.DrugFeeViews.Count;
                            }
                            //drugRecordsList.Add(drugRecord);
                            drugRecordViewsList.Add(drugRecord);
                        }
                    });


                    if (drugRecords != null)
                    {
                        //inPatient.InPatientDrugRecords = drugRecords.ToList();
                        inPatient.InPatientDrugRecordViews.AddRange(drugRecordViewsList);
                        if (inPatientViewList.Capacity - inPatientViewList.Count < inPatient.InPatientDrugRecordViews.Count)
                        {
                            inPatientViewList.Capacity += inPatient.InPatientDrugRecordViews.Count;
                        }
                        inPatientViewList.Add(inPatient);

                    }
                    context.Dispose();

                });
                #region Parallel

                //Parallel.For(0, maxDays, (i, state) =>
                //{
                //    //do
                //    //{
                //    var startTime2 = startTime.AddDays( i * durationDay);
                //    var endTime2 = startTime2.AddDays( durationDay);

                //    if (endTime2 > endTime)
                //    {
                //        endTime2 = endTime;
                //    }
                //    PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();
                //    IInnerRepository innerRepository = new InnerRepository(new Domain2dot1InnerFactory(context));
                //    //根据DrugFee中的收费时间获取入院患者集合（含在取定时间范围之前的患者）
                //    //var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime2, endTime2);
                //    //inPatientFromDrugRecordTotalList.AddRange(inPatientFromDrugRecordList);

                //    //获取各科室抗菌药物使用人数
                //    //var antibioticDepartmentPerson = inPatientFromDrugRecordList.SelectMany(i => i.AntibioticDepartmentPersonList(startTime, endTime)).ToList().GroupBy(a => a.DepartmentID).Select(g => new DepartmentAntibioticUsageRateDomain { DepartmentID = g.Key, AntibioticPerson = g.Sum(a => a.PersonNumber) }).ToList();

                //    var departmentPersonList = innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime2, endTime2);
                //    //var departmentPersonList = await this.innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPersonAsync(startTime2, endTime2);
                //    departmentPersonTotoalList.AddRange(departmentPersonList);



                //    //var result1 = antibioticDepartmentPerson.Join(departmentPersonList, antibioticPerson => antibioticPerson.DepartmentID, departmentPerson => departmentPerson.DepartmentID, (antibioticPerson, departmentPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = antibioticPerson.DepartmentID, AntibioticPerson = antibioticPerson.AntibioticPerson, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

                //    //result = result1.Join(departments, result11 => result11.DepartmentID, department => department.Origin_DEPT_ID, (result11, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = result11.DepartmentID, DepartmentName = department.DepartmentName, AntibioticPerson = result11.AntibioticPerson, RegisterPerson = result11.RegisterPerson }).ToList();

                //    //result = result2.Join(antibioticDepartmentPerson, departmentPerson => departmentPerson.DepartmentID, antibioticPerson => antibioticPerson.DepartmentID, (departmentPerson, antibioticPerson) => new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = departmentPerson.DepartmentName, RegisterPerson = departmentPerson.RegisterPerson, AntibioticPerson = antibioticPerson.AntibioticPerson }).ToList();

                //    //startTime2 = endTime2;
                //    //endTime2 = startTime2.AddDays(durationDay);
                //    //}
                //    //while (startTime2 < endTime);

                //});
                #endregion

                inPatientViewList.TrimExcess();
                var antibioticPerson2 = inPatientViewList.Select(a => a.AntibioticDepartmentPerson(startTime, endTime)).ToList();
                antibioticPersonTotalList.AddRange(antibioticPerson2);
                //需再group
                var departmentPersonResult = departmentPersonTotoalList.GroupBy(a => a.DepartmentID).Select(g => new { DepartmentID = g.Key, InPatientNumber = g.Sum(b => b.InPatientNumber) });
                var antibioticPersonList = antibioticPersonTotalList.GroupBy(a => a.DepartmentID).Select(g => new AntibioticPerson { DepartmentID = g.Key, AntibioticPatientNumber = g.Sum(b => b.AntibioticPatientNumber) });
                //join
                result = departmentPersonResult.Join(departments, departmentPerson => departmentPerson.DepartmentID, department => department.Origin_DEPT_ID, (departmentPerson, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = department.DepartmentName, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

                result = (from departmentPerson in result
                          join antibioticPerson in antibioticPersonList on departmentPerson.DepartmentID equals antibioticPerson.DepartmentID into gj
                          from subgj in gj.DefaultIfEmpty()
                          select new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = departmentPerson.DepartmentName, RegisterPerson = departmentPerson.RegisterPerson, AntibioticPerson = subgj == null ? 0 : subgj.AntibioticPatientNumber }).ToList();
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(String.Format("数据处理出错!{0}", e.Message));
            }

            return result;
        }

        public async Task<List<DepartmentAntibioticUsageRateDomain>> GetDepartmentAntibioticUsageRateDomainAsync(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticUsageRateDomain>();

            var departmentPersonTotoalList = new List<OutDepartmentPerson>();
            var inPatientFromDrugRecordTotalList = new List<InPatient>();
            var antibioticPersonTotalList = new List<AntibioticPerson>();
            //同期出院人数
            departmentPersonTotoalList = innerRepository.CreateOutDepartmentPerson().GetOutDepartmentPerson(startTime, endTime);

            //获取科室集合

            var departments = this.innerFactory.CreateDepartment().GetDepartment();

            //抗菌药物使用人的出院病人集合
            var inPatientFromDrugRecordList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientInDruation(startTime, endTime);
            //var inPatientList = this.innerFactory.CreateInPatientInDuration().GetInPatientInDruation(startTime, endTime);
            this.innerFactory.Dispose();
            //var inPatientViews = inPatientFromDrugRecordList.Select(a => new InPatientView { InPatientID = a.InPatientID, CaseNumber = a.CaseNumber, InDate = a.InDate, Origin_DEPT_ID = a.Origin_DEPT_ID, OutDate = a.OutDate, Origin_IN_DEPT = a.Origin_IN_DEPT }).AsParallel().ToList();

            var inPatientViewList = new List<InPatientView>();
            //foreach操作
            //foreach (var inPatient in inPatientFromDrugRecordList)
            //创建三个Task去查InPatient,将InPatient分成三组
            var inPatientTotalCount = inPatientFromDrugRecordList.Count;
            var taskArray = new Task<List<AntibioticPerson>>[5];

            int durationCount = inPatientTotalCount / taskArray.Length;
            var inPatientList10 = new List<InPatient>[taskArray.Length + 1];
            int i = 0;
            for (i = 0; i < taskArray.Length - 1; i++){
                inPatientList10[i] = inPatientFromDrugRecordList.Skip(i * durationCount).Take(durationCount).ToList();
            }
            inPatientList10[i] = inPatientFromDrugRecordList.Skip(i * durationCount).ToList();
            //inPatientList10[taskArray.Length + 1] = null;
            //var inPatientList1 = inPatientFromDrugRecordList.Take(durationCount).ToList();
            //var inPatientList2 = inPatientFromDrugRecordList.Skip(durationCount).Take(durationCount).ToList();
            //var inPatientList3 = inPatientFromDrugRecordList.Skip(durationCount * 2).Take(durationCount).ToList();
            //var inPatientList4 = inPatientFromDrugRecordList.Skip(durationCount * 3).ToList();

            for (int j = 0; j < taskArray.Length; j++)
            {
                taskArray[j] = Task<List<AntibioticPerson>>.Factory.StartNew(() => GetAntibioticPersonList(startTime, endTime, inPatientList10[j]));
            }
            //taskArray[0] = Task<List<AntibioticPerson>>.Factory.StartNew(() => GetAntibioticPersonList(startTime, endTime, inPatientList1));
            //taskArray[1] = Task<List<AntibioticPerson>>.Factory.StartNew(() => GetAntibioticPersonList(startTime, endTime, inPatientList2));
            //taskArray[2] = Task<List<AntibioticPerson>>.Factory.StartNew(() => GetAntibioticPersonList(startTime, endTime, inPatientList3));
            //antibioticPersonTotalList.AddRange(taskArray[0].Result);
            //antibioticPersonTotalList.AddRange(taskArray[1].Result);
            //antibioticPersonTotalList.AddRange(taskArray[2].Result);

            for(int k = 0; k < taskArray.Length; k++)
            {
                antibioticPersonTotalList.AddRange(taskArray[k].Result);
            }
            var action = new Action(() => { });
            await Task.Run(action);
            #region TaskAction 失败

            //var TaskAction = new Action(() => {
            //    Parallel.ForEach(inPatientFromDrugRecordList, async (inPatient, state, index) =>
            //    {
            //        PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();

            //        var inPatientDrugRecords = new List<InPatientDrugRecord>();
            //        var drugRecords = await context.InPatientDrugRecords.Where(a => a.InPatientID == inPatient.InPatientID).ToListAsync();
            //        foreach (var drugRecord in drugRecords)
            //        {
            //            var drugFees = await context.DrugFees.Where(a => a.InPatientDrugRecordID == drugRecord.InPatientDrugRecordID && a.ChargeTime >= startTime && a.ChargeTime < endTime).ToListAsync();
            //            drugRecord.DrugFees = drugFees;
            //            inPatientDrugRecords.Add(drugRecord);
            //        }
            //        context.Dispose();

            //        inPatient.InPatientDrugRecords = inPatientDrugRecords;
            //        //计算出当前inPatient中AntibioticPerson
            //        var antibioticPerson = inPatient.AntibioticDepartmentPerson(startTime, endTime);
            //        antibioticPersonTotalList.Add(antibioticPerson);
            //    }
            //);
            //});
            //var Task1 = new Task(TaskAction);
            //await Task.Run(TaskAction);

            #endregion

            //var antibioticPerson2 = inPatientViewList.Select(a => a.AntibioticDepartmentPerson(startTime, endTime)).ToList();
            //antibioticPersonTotalList.AddRange(antibioticPerson2);
            //需再group
            var departmentPersonResult = departmentPersonTotoalList.GroupBy(a => a.DepartmentID).Select(g => new { DepartmentID = g.Key, InPatientNumber = g.Sum(b => b.InPatientNumber) });
            var antibioticPersonList = antibioticPersonTotalList.GroupBy(a => a.DepartmentID).Select(g => new AntibioticPerson { DepartmentID = g.Key, AntibioticPatientNumber = g.Sum(b => b.AntibioticPatientNumber) });
            //join
            result = departmentPersonResult.Join(departments, departmentPerson => departmentPerson.DepartmentID, department => department.Origin_DEPT_ID, (departmentPerson, department) => new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = department.DepartmentName, RegisterPerson = departmentPerson.InPatientNumber }).ToList();

            result = (from departmentPerson in result
                      join antibioticPerson in antibioticPersonList on departmentPerson.DepartmentID equals antibioticPerson.DepartmentID into gj
                      from subgj in gj.DefaultIfEmpty()
                      select new DepartmentAntibioticUsageRateDomain { DepartmentID = departmentPerson.DepartmentID, DepartmentName = departmentPerson.DepartmentName, RegisterPerson = departmentPerson.RegisterPerson, AntibioticPerson = subgj == null ? 0 : subgj.AntibioticPatientNumber }).ToList();
            return result;
        }
        private List< AntibioticPerson> GetAntibioticPersonList(DateTime startTime, DateTime endTime, List<InPatient> inPatientList)
        {
            var result = new List<AntibioticPerson>();

            PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();
            foreach(var inPatient in inPatientList)
            {
                var inPatientDrugRecords = new List<InPatientDrugRecord>();
                var drugRecords = context.InPatientDrugRecords.Where(a => a.InPatientID == inPatient.InPatientID).ToList();
                foreach (var drugRecord in drugRecords)
                {
                    var drugFees = context.DrugFees.Where(a => a.InPatientDrugRecordID == drugRecord.InPatientDrugRecordID && a.ChargeTime >= startTime && a.ChargeTime < endTime).ToList();
                    drugRecord.DrugFees = drugFees;
                    inPatientDrugRecords.Add(drugRecord);
                }
                inPatient.InPatientDrugRecords = inPatientDrugRecords;
                //计算出当前inPatient中AntibioticPerson
                var antibioticPerson = inPatient.AntibioticDepartmentPerson(startTime, endTime);
                result.Add(antibioticPerson);
            }
            
            context.Dispose();

           
            return result;
        }

        private  AntibioticPerson GetAntibioticPerson(DateTime startTime, DateTime endTime,InPatient inPatient)
        {
            var result = new AntibioticPerson();
            PhMS2dot1DomainContext context = new PhMS2dot1DomainContext();

            var inPatientDrugRecords = new List<InPatientDrugRecord>();
            var drugRecords =  context.InPatientDrugRecords.Where(a => a.InPatientID == inPatient.InPatientID).ToList();
            foreach (var drugRecord in drugRecords)
            {
                var drugFees =  context.DrugFees.Where(a => a.InPatientDrugRecordID == drugRecord.InPatientDrugRecordID && a.ChargeTime >= startTime && a.ChargeTime < endTime).ToList();
                drugRecord.DrugFees = drugFees;
                inPatientDrugRecords.Add(drugRecord);
            }
            context.Dispose();

            inPatient.InPatientDrugRecords = inPatientDrugRecords;
            //计算出当前inPatient中AntibioticPerson
            result = inPatient.AntibioticDepartmentPerson(startTime, endTime);
            return result;
        }
    }
}
