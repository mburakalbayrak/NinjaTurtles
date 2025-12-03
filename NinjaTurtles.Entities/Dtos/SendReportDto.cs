namespace NinjaTurtles.Entities.Dtos
{
    public class SendReportDto
    {
        public Guid? GuidId { get; set; }
        public string NameSurName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }

    }
}
