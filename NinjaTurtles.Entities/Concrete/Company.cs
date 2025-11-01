using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class Company : BaseEntity<int>, IEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
