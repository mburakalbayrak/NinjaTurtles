using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class CustomerQrVerification : BaseEntity<int>, IEntity
    {
        public int CustomerId { get; set; }
        public int QrCodeMainId { get; set; }
        public int VerificationTypeId { get; set; }
        public int Code { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool VerifyDate { get; set; }
    }
}
