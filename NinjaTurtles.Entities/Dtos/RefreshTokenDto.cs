using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Dtos
{
    public class RefreshTokenDto : IDto
    {
        public string RefreshToken { get; set; }
    }
}
