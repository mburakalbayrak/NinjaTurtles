using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Abstract
{
    public interface IProductService
    {
        IDataResult<Product> GetById(int productId);
        IDataResult<List<Product>> GetList();

        IDataResult<List<Product>> GetListByCategory(int categoryId);

        IDataResult<Product> Add(Product product);

        IResult Delete(Product product);

        IDataResult<Product> Update(Product product);
    }
}
