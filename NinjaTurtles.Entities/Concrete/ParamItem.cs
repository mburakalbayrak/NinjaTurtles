using NinjaTurtles.Core.Entities;
using NinjaTurtles.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Entities.Concrete
{
    public class ParamItem : BaseEntity<int>, IEntity
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public ParamEnums ParamId { get; set; }
        public int? ParentId { get; set; }
    }

   
}
