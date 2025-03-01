﻿using NinjaTurtles.Core.DataAccess.EntityFramework;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework.Contexts;
using NinjaTurtles.Entities.Concrete;

namespace NinjaTurtles.DataAccess.Concrete.EntityFramework
{
    public class EfCompanyOrderDetailDal : EfEntityRepositoryBase<CompanyOrderDetail, NinjaTurtlesContext>, ICompanyOrderDetailDal
    {

    }
}
