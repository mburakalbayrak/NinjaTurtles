using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IDataResult<Product> Add(Product product)
        {
            return new SuccessDataResult<Product>(_productDal.Add(product), Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.Id == productId));
        }

        public IDataResult<List<Product>> GetList()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList());
        }

        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId));
        }

        public IDataResult<Product> Update(Product product)
        {
            return new SuccessDataResult<Product>(_productDal.Update(product), Messages.ProductDeleted);
        }
    }
}
