using NinjaTurtles.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace NinjaTurtles.Entities.Concrete
{
    public class CustomerQrVerification : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VerificationTypeId { get; set; }
        public int Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? VerifyDate { get; set; }
    }
}
