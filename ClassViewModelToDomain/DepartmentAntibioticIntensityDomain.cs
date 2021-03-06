﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassViewModelToDomain
{
    public class DepartmentAntibioticIntensityDomain : IEquatable<DepartmentAntibioticIntensityDomain>
    {
        public DepartmentAntibioticIntensityDomain()
        {
            this.AntibioticDdd = 0;
            this.PersonNumberDays = 0;
        }
        private Decimal antibioticDdd;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        //住院抗菌药物消耗量（累计DDD数
        public Decimal AntibioticDdd {
            get { return Decimal.Round(this.antibioticDdd, 2); } 
            set { this.antibioticDdd = value; }
        }
        //同期收治患者人天数：同期出院患者人数×同期出院患者平均住院天数
        public int PersonNumberDays { get; set; }

        public Decimal IntensityRate
        {
            get
            {
                return this.PersonNumberDays != 0
                   ? Decimal.Round((Decimal)this.AntibioticDdd  / (Decimal)this.PersonNumberDays, 2)
                  : 0;
            }
        }

        public bool Equals(DepartmentAntibioticIntensityDomain other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return DepartmentID.Equals(other.DepartmentID);
        }
        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashDepartmentID = DepartmentID.GetHashCode();

            //Get hash code for the Code field.

            //Calculate the hash code for the product.
            return hashDepartmentID;
        }
    }
}
