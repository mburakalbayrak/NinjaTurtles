﻿using NinjaTurtles.Core.Entities;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrCodeMain:BaseEntity<Guid>, IEntity
    {
        public int? CustomerId { get; set; }
        public string? RedirectUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyOrderDetail { get; set; }
        public int? DetailTypeId { get; set; }
    }
}
