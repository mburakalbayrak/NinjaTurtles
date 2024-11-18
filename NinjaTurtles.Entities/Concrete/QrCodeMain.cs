using NinjaTurtles.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Entities.Concrete
{
    public class QrCodeMain:BaseEntity<Guid>, IEntity
    {
        public int? QrId { get; set; }
        public int? CustomerId { get; set; }
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
        public int? QrDetailId { get; set; }
    }
}
