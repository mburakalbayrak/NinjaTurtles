using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Entities.Dtos
{
    public class VerifyCustomerEmailDto
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
