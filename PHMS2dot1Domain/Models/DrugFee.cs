﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PhMS2dot1Domain.Models
{
    [Table("DrugFees")]
    public class DrugFee
    {
        #region Domain

        [Key]
        [Display(Name = "药物费用ID")]
        public virtual Guid DrugFeeID { get; set; }
        [Display(Name = "原HIS费用表ID")]
        public virtual Guid? Origin_ID { get; set; }
        [Display(Name = "药物记录ID")]
        public virtual Guid InPatientDrugRecordID { get; set; }
        [Display(Name = "药物单价")]
        public virtual Decimal UnitPrice { get; set; }
        [Display(Name = "原HIS药物单位")]
        public virtual string Origin_Unit { get; set; }
        [Display(Name = "药物数量")]
        public virtual Decimal Quantity { get; set; }
        [Display(Name = "实际总价")]
        public virtual Decimal ActualPrice { get; set; }
        [Display(Name = "收费时间")]
        public virtual DateTime ChargeTime { get; set; }
        public virtual InPatientDrugRecord InPatientDrugRecord { get; set; }
        #endregion

        #region 扩展方法
        #region 取定时间内的药物费用

        public Decimal ActualPriceInDuration(DateTime startTime, DateTime endTime)
        {
            return this.ChargeTime >= startTime && this.ChargeTime < endTime
                ? this.ActualPrice
                : 0;
        }
        #endregion
        #region 取定时间内的药物数量
        public Decimal QuantityInDuration(DateTime startTime, DateTime endTime)
        {
            return this.ChargeTime >= startTime && this.ChargeTime < endTime
                ? this.Quantity
                : 0;
        }
        #endregion
        #endregion

    }
}
