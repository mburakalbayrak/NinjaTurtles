using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class Product: BaseEntity<int> ,IEntity
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
    }
}
