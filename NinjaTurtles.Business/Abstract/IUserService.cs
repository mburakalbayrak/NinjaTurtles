using NinjaTurtles.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);

        int Add(User user);

        User GetByMail(string email);
    }
}
