using Dapper;
using sup_traders.Access;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Business.Repositories
{
    public interface IUserRepository
    {
        public Return<User> RegisterUser(User u);
        public Return<User> GetUser(int id);
        public bool UpdateUserBalance(int id, decimal amount, OrgType orgType, decimal price = 1);
    }

    public class UserRepository(IUserAccessor userAccessor) : IUserRepository
    {
        private readonly IUserAccessor _userAccessor = userAccessor;

        public Return<User> RegisterUser(User u)
        {
            if (_userAccessor.RegisterUser(u))
            {
                return new Return<User>()
                {
                    Data = u,
                    Message = "User created successfully!",
                };
            }
            else
            {
                return new Return<User>()
                {
                    Data = u,
                    Message = "User cannot be created!",
                };
            }
        }
        public Return<User> GetUser(int id)
        {
            var u = _userAccessor.GetUser(id);
            if (u != null)
            {
                return new Return<User>()
                {
                    Data = u,
                    Message = "Found in DB",
                };
            }


            return new Return<User>()
            {
                Data = null,
                Message = "User not found!",
            };
        }
        public bool UpdateUserBalance(int id, decimal amount, OrgType orgType, decimal price = 1)
        {
            return _userAccessor.UpdateUserBalance(id, amount, orgType, price);
        }
    }
}
