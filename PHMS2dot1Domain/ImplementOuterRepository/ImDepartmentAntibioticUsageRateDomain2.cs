using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;
using PhMS2dot1Domain.ViewModels;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImDepartmentAntibioticUsageRateDomain2 : IDepartmentAntibioticUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        private readonly IInnerRepository innerRepository;
        PhMS2dot1DomainContext context = null;

        public ImDepartmentAntibioticUsageRateDomain2(IDomain2dot1InnerFactory factory)
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

                #region Parallel

                inPatientViewList.Capacity = 2000;
                Parallel.ForEach(inPatientViews, (inPatient, state, index) =>
                {
                    //var drugRecordsList = new List<InPatientDrugRecord>();
                    var drugRecordViewsList = new List<InPatientDrugRecordView>();
                    drugRecordViewsList.Capacity = 1000;
                    inPatient.InPatientDrugRecordViews = new List<InPatientDrugRecordView>();

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

        public Task<List<DepartmentAntibioticUsageRateDomain>> GetDepartmentAntibioticUsageRateDomainAsync(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    
    }
}
