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
        public Return<bool> UpdateUserBalance(int id, float amount, OrgType orgType);
        public Return<List<User>> LoadUsers();
    }

    public class UserRepository : IUserRepository
    {
        private readonly GameManager _gm;
        private readonly IUserAccessor _userAccessor;

        public UserRepository(GameManager gm, IUserAccessor userAccessor)
        {
            _gm = gm;
            _userAccessor = userAccessor;
        }

        public Return<User> RegisterUser(User u)
        {
            if (_gm.RegisterUser(u))
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

            return new Return<User>()
            {
                Data = u,
                Message = "User cannot be created!",
            };
        }

        public Return<User> GetUser(int id)
        {
            var user = _gm.Users.FirstOrDefault(item => item.id == id);

            if (user != null)
            {
                return new Return<User>()
                {
                    Data = user,
                    Message = "Found in GM",
                };
            } else
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
            }

            return new Return<User>()
            {
                Data = null,
                Message = "User not found!",
            };
        }

        public Return<bool> UpdateUserBalance(int id, float amount, OrgType orgType)
        {
            var u = GetUser(id) ?? null;

            if (u != null && u.Data != null)
            {
                if (u.Data.UpdateBalance(amount, orgType))
                {
                    return new Return<bool>() 
                    {
                        Data = _userAccessor.UpdateUserBalance(id, u.Data.balance),
                        Message = $"{u.Data.name}, {orgType}, {amount}, new balance: {u.Data.balance}",
                    };
                }
            }

            return new Return<bool>()
            {
                Data = false,
                Message = u?.Message ?? "",
            };
        }

        public Return<List<User>> LoadUsers()
        {
            var msg = string.Empty;
            var users = _userAccessor.LoadUsers();

            if (users == null)
            {
                msg = "There is no user registered!";
            }
            else
            {
                foreach (var u in users)
                {
                    _gm.RegisterUser(u);
                }
            }

            return new Return<List<User>>()
            {
                Message = msg,
                Data = users,
            };
        }
    }
}
