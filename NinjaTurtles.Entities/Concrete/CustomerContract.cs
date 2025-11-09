using NinjaTurtles.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Entities.Concrete
{
    public class CustomerContract : BaseEntity<int>, IEntity
    {
        public int CustomerId { get; set; }
        public string ContractName { get; set; }
        public string FileName { get; set; }
        public DateTime VerifyDate { get; set; }
        public int VerifyState { get; set; }
        public int VerifyCode { get; set; }
        
    }
}
