using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrCodeMain : BaseEntity<Guid>, IEntity
    {
        public int? CustomerId { get; set; }
        public string? QrUrl { get; set; }
        public string? RedirectUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyOrderDetailId { get; set; }
        public int? DetailTypeId { get; set; }
    }
}
