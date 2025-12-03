using NinjaTurtles.Core.Entities;
using System.ComponentModel.DataAnnotations;


namespace NinjaTurtles.Entities.Concrete
{
    public class SupportTask : IEntity
    {
        [Key]
        public int Id { get; set; }
        public Guid? GuidId { get; set; }
        public string NameSurName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
