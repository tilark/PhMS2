using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Models
{
    [Table("ImportDataLogs")]
    public class ImportDataLog
    {
        #region Domain

       
        [Key]
        [Display(Name = "数据转入日志ID")]
        public long ImportDataLogsID { get; set; }

        [Display(Name = "源数据名称")]
        public string SourceDatabaseName { get; set; }

        [Display(Name = "源数据表名")]
        public string SourceTableName { get; set; }

        [Display(Name = "本地表名称")]
        public string LocalTableName { get; set; }

        [Display(Name = "取数时段起点")]
        public DateTime StartTime { get; set; }

        [Display(Name = "取数时段终点")]
        public DateTime EndTime { get; set; }

        [Display(Name = "源记录数")]
        public long SourceRecordCount { get; set; }

        [Display(Name = "成功导入记录数")]
        public long SuccessImportRecordCount { get; set; }

        [Display(Name = "读取开始时间")]
        public DateTime ReadStartTime { get; set; }

        [Display(Name = "读取结束时间")]
        public DateTime ReadEndTime { get; set; }

        [Display(Name = "写入开始时间")]
        public DateTime WriteStartTime { get; set; }
        [Display(Name = "写入结束时间")]
        public DateTime WriteEndTime { get; set; }

        [Display(Name = "错误信息")]
        public string ErrorMessage { get; set; }

        [Display(Name = "备注")]
        public string Remarks { get; set; }
        #endregion
    }
}
