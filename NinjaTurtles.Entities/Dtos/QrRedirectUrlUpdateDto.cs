
namespace NinjaTurtles.Entities.Dtos
{
    public class QrRedirectUrlUpdateDto
    {
        public Guid QrMainId { get; set; }
        public string RedirectUrl { get; set; } 
        public int Code { get; set; }
    }
}
