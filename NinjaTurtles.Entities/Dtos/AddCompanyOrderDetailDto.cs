namespace NinjaTurtles.Entities.Dtos
{
    public class AddCompanyOrderDetailDto
    {
        public int CompanyOrderId { get; set; }
        public double LicenceUnitPrice { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }

    }
}
