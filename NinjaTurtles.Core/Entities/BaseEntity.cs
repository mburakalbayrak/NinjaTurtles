using System.ComponentModel.DataAnnotations;

namespace NinjaTurtles.Core.Entities
{
    public class BaseEntity<TId>
    {
        [Key]
        public TId Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int? ModifiedBy { get; set; }
    }
}
