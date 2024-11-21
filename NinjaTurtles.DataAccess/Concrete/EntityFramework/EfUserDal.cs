using NinjaTurtles.Core.DataAccess.EntityFramework;
using NinjaTurtles.Core.Entities.Concrete;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, NinjaTurtlesContext>, IUserDal
    {
        // kullanıcının rollerini çekeceğiz
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new NinjaTurtlesContext())
            {
                var result = from operationClaim in context.OperationClaim
                             join userOperationClaim in context.UserOperationClaim
                                on operationClaim.Id equals userOperationClaim.OperationClaimId
                                    where userOperationClaim.UserId == user.Id
                                    select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name};

                return result.ToList();
            }
        }
    }
}
