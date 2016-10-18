using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Interface;
using PHMS2Domain.Implement;
using PHMS2Domain.Models;
using ClassViewModelToDomain;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Factory
{
    public class DomainInnerFactory : IDomainInnerFactory
    {
        PHMS2DomainContext context = null;

        #region Structer
        public DomainInnerFactory() : this(new PHMS2DomainContext())
        {

        }
        public DomainInnerFactory(PHMS2DomainContext dbContext)
        {
            this.context = dbContext;
        }

        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (this.context != null)
                    {
                        this.context.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DomainInternalFactory() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion
        public IPrescriptionInDuration CreatePrescrtionInDuration()
        {
            IPrescriptionInDuration result = null;
            result = new ImPrescriptionInDuration.GetPrescriptionInDurationList(this.context);
            return result;
        }

        public IRegisterInDuration CreateRegisterFromPrescription(EnumOutPatientCategories categories)
        {
            IRegisterInDuration result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImRegisterFromPerscription.GetOutPatientEmergencyRegisterFromPrescription(this.context);
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImRegisterFromPerscription.GetOutPatientRegisterFromPrescription(this.context);
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImRegisterFromPerscription.GetEmergencyRegisterFromPrescription(this.context);
                    break;

            }
            return result;
        }

        public IRegisterInDuration CreateRegisterInDuration(EnumOutPatientCategories categories)
        {
            IRegisterInDuration result = null;
            switch (categories)
            {
                case EnumOutPatientCategories.OUTPATIENT_EMERGEMENT:
                    result = new ImRegisterInDuration.GetOutPatientEmergencyRegisterInDuration(this.context);
                    break;
                case EnumOutPatientCategories.OUTPATIENT:
                    result = new ImRegisterInDuration.GetOutPatientRegisterInDuration(this.context);
                    break;
                case EnumOutPatientCategories.EMERGEMENT:
                    result = new ImRegisterInDuration.GetEmergencyRegisterInDuration(this.context);
                    break;

            }
            return result;
        }      
      
    }
}
