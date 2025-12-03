using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrLog : BaseEntity<int>, IEntity
    {
        public int LogTypeId { get; set; }
        public Guid QrCodeMainId { get; set; }
        public string IpAddress { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
