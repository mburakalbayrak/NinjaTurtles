using NinjaTurtles.Core.DataAccess.EntityFramework;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework.Contexts;
using NinjaTurtles.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.DataAccess.Concrete.EntityFramework
{
    public class EfQrCodeMain:EfEntityRepositoryBase<QrCodeMain, NinjaTurtlesContext>, IQrCodeMain
    {
    }
}
