using PhMS2dot1Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Models;

namespace PhMS2dot1Domain.Implement
{
    public class ImDepartment : IDepartment
    {
        private readonly PhMS2dot1DomainContext context;

        public ImDepartment(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public List<Department> GetDepartment()
        {
            return this.context.Departments.ToList();
        }
    }
}
