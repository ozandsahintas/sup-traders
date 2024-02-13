﻿using Dapper;
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
        public Return<List<User>> LoadUsers();
    }

    public class UserRepository(GameManager gm, IUserAccessor userAccessor) : IUserRepository
    {
        private readonly GameManager _gm = gm;
        private readonly IUserAccessor _userAccessor = userAccessor;

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
        public bool UpdateUserBalance(int id, decimal amount, OrgType orgType, decimal price = 1)
        {
            return _userAccessor.UpdateUserBalance(id, amount,orgType,price);
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
