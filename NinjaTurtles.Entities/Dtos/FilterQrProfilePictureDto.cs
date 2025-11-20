using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Entities.Dtos
{
    public class FilterQrProfilePictureDto
    {
        public Guid Id { get; set; }

        public int? DetailTypeId { get; set; } = 1;

    }
}
