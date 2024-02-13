using Dapper;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;
using System.Collections.Generic;

namespace sup_traders.Access
{
    public interface IShareAccessor
    {
        public bool RegisterShare(Share s);
        public List<Share> LoadShares();
        public bool UpdateShare(string code, decimal amount);
        public decimal GetShareValue(string code);
    }

    public class ShareAccessor(ConnectionHelper connectionHelper) : IShareAccessor
    {
        private readonly ConnectionHelper _connectionHelper = connectionHelper;

        public bool RegisterShare(Share s)
        {
            var query = "INSERT INTO Shares (code, count, price) " +
            "VALUES (@code, @count, @price)";

            var parameters = new DynamicParameters();
            parameters.Add("code", s.code);
            parameters.Add("count", s.count);
            parameters.Add("price", s.price);

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
        public List<Share> LoadShares()
        {
            var query = "SELECT * FROM Shares";
            var connection = _connectionHelper.CreateSqlConnection();
            var shares = connection.Query<Share>(query);
            return shares.ToList();
        }
        public bool UpdateShare(string code, decimal amount)
        {
            var query = "UPDATE Shares " +
                        "SET count = count - @amount " +
                        "WHERE code = @code";

            var parameters = new DynamicParameters();
            parameters.Add("code", code);
            parameters.Add("amount", amount);

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
        public decimal GetShareValue(string code)
        {
            var query = "SELECT price FROM Shares " +
                        "WHERE code = @code";

            var parameters = new DynamicParameters();
            parameters.Add("code", code);

            var connection = _connectionHelper.CreateSqlConnection();
            try
            {
                var price = connection.QuerySingle<decimal>(query, parameters);
                return price;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
