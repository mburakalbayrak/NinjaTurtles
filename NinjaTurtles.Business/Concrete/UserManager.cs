using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Entities.Concrete;
using NinjaTurtles.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace NinjaTurtles.Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public int Add(User user)
        {
            return _userDal.Add(user).Id;
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(x => x.Email == email && x.IsActive && !x.IsDeleted);
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public User GetByRefreshToken(string refreshToken)
        {
            return _userDal.Get(x => x.RefreshToken == refreshToken && x.IsActive && !x.IsDeleted);
        }

        public void UpdateRefreshToken(User user)
        {
            _userDal.Update(user);
        }
    }
}
