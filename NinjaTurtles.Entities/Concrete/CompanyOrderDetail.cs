using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class CompanyOrderDetail: BaseEntity<int>, IEntity
    {
        public int CompanyOrderId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
