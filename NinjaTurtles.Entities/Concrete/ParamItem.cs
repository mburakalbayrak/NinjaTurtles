using NinjaTurtles.Core.Entities;
using NinjaTurtles.Core.Entities.Enums;

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
