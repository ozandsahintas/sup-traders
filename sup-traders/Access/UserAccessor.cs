using Dapper;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;
using System.Collections.Generic;

namespace sup_traders.Access
{
    public interface IUserAccessor
    {
        public bool RegisterUser(User u);
        public User? GetUser(int id);
        public bool UpdateUserBalance(int id, decimal amount, OrgType orgType, decimal price);
        public List<User> LoadUsers();
    }

    public class UserAccessor(ConnectionHelper connectionHelper) : IUserAccessor
    {
        private readonly ConnectionHelper _connectionHelper = connectionHelper;

        public bool RegisterUser(User u)
        {
            var query = "INSERT INTO Users (name, balance) " +
            "VALUES (@name, @balance)";

            var parameters = new DynamicParameters();
            parameters.Add("name", u.name);
            parameters.Add("balance", u.balance);

            using var connection = _connectionHelper.CreateSqlConnection();
            try
            {
                connection.Execute(query, parameters);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public User GetUser(int id)
        {
            var query = "SELECT * FROM Users " +
            "WHERE [id] = @id";

            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            var connection = _connectionHelper.CreateSqlConnection();
            var u = connection.Query<User>(query, parameters).FirstOrDefault();
            return u;
        }
        public bool UpdateUserBalance(int id, decimal amount, OrgType orgType, decimal price)
        {
            var u = GetUser(id);
            if(u != null)
            {
                var nb = CalculateNewBalance(u.balance, amount, orgType, price);
                if (nb >= 0)
                {
                    var query = "UPDATE Users " +
                                "SET [balance] = @balance " +
                                "WHERE [id] = @id; ";

                    var parameters = new DynamicParameters();
                    parameters.Add("id", id);
                    parameters.Add("balance", nb);

                    var connection = _connectionHelper.CreateSqlConnection();
                    try
                    {
                        connection.Execute(query, parameters);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        private decimal CalculateNewBalance(decimal balance, decimal amount, OrgType t, decimal price)
        {
            switch (t)
            {
                case OrgType.DEPOSIT:
                case OrgType.SELL:
                    if (amount != 0)
                    {
                        balance += amount * price;
                        return balance;
                    }
                    break;
                case OrgType.WITHDRAW:
                case OrgType.BUY:
                    if (balance >= amount * price)
                    {
                        balance -= amount * price;
                        return balance;
                    }
                    break;
                default:
                    return -1;
            }

            return -1;

        }
        public List<User> LoadUsers()
        {
            var query = "SELECT * FROM Users";
            var connection = _connectionHelper.CreateSqlConnection();
            var users = connection.Query<User>(query);
            return users.ToList();
        }
    }
}
