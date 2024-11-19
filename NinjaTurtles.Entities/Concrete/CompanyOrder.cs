using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class CompanyOrder : BaseEntity<int>, IEntity
    {
        public string Name { get; set; }
    }
}
