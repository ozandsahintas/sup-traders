using Dapper;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;
using System.Collections.Generic;

namespace sup_traders.Access
{
    public interface IShareAccessor
    {
        public bool RegisterShare(Share s);
        public bool UpdateShare(string code, decimal amount);
        public Share? GetShare(string code);
    }

    public class ShareAccessor(ConnectionHelper connectionHelper) : IShareAccessor
    {
        private readonly ConnectionHelper _connectionHelper = connectionHelper;

        public bool RegisterShare(Share s)
        {
            var query = "INSERT INTO Shares (code, count, price, baseCount, basePrice) " +
            "VALUES (@code, @count, @price, @count, @price)";

            var parameters = new DynamicParameters();
            parameters.Add("code", s.code);
            parameters.Add("count", s.count);
            parameters.Add("price", s.price);

            using var connection = _connectionHelper.CreateSqlConnection();
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Share? GetShare(string code)
        {
            var query = "SELECT price, count, baseCount FROM Shares " +
                        "WHERE code = @code";

            var parameters = new DynamicParameters();
            parameters.Add("code", code);

            var connection = _connectionHelper.CreateSqlConnection();
            try
            {
                var s = connection.Query<Share>(query, parameters).FirstOrDefault() ?? null;
                return s;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
