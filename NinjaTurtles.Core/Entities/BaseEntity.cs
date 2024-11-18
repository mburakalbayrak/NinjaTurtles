using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Entities
{
    public class BaseEntity<TId>
    {
        [Key]
        public TId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public int? ModifiedBy { get; set; }
    }
}
