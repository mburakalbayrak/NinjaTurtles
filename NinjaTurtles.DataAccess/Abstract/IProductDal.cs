using NinjaTurtles.Core.DataAccess;
using NinjaTurtles.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.DataAccess.Abstract
{
    public interface IProductDal:IEntityRepository<Product>
    {
    }
}
