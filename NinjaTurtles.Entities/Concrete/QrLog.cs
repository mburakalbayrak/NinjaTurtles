using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrLog : BaseEntity<int>, IEntity
    {
        public int LogTypeId { get; set; }
        public int QrCodeMainId { get; set; }
        public string IpAddress { get; set; }
    }
}
