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
        public bool UpdateUserBalance(int id, float balance);
        public List<User> LoadUsers();
    }

    public class UserAccessor : IUserAccessor
    {
        private readonly ConnectionHelper _connectionHelper;

        public UserAccessor(ConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public bool RegisterUser(User u)
        {
            var query = "INSERT INTO Users (name, balance) " +
            "VALUES (@name, @balance)";

            var parameters = new DynamicParameters();
            parameters.Add("name", u.name);
            parameters.Add("balance", Double.Round(u.balance, 2));

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

        public bool UpdateUserBalance(int id, float balance)
        {
            var query = "UPDATE Users " +
                "SET [balance] = @balance " +
                "WHERE [id] = @id; ";

            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("balance", balance);

            var connection = _connectionHelper.CreateSqlConnection();
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

        public List<User> LoadUsers()
        {
            var query = "SELECT * FROM Users";
            var connection = _connectionHelper.CreateSqlConnection();
            var users = connection.Query<User>(query);
            return users.ToList();
        }
    }
}
