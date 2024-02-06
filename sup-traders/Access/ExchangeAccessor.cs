using Dapper;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Access
{
    public interface IExchangeAccesor
    {
        public bool RegisterShare(Share s);
        public List<Share> LoadShares();
    }

    public class ExchangeAccesor : IExchangeAccesor
    {
        private readonly ConnectionHelper _connectionHelper;

        public ExchangeAccesor(ConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public bool RegisterShare(Share s)
        {
            var query = "INSERT INTO Shares (code, count, price) " +
            "VALUES (@code, @count, @price)";

            var parameters = new DynamicParameters();
            parameters.Add("code", s.code);
            parameters.Add("count", s.count);
            parameters.Add("price", Double.Round(s.price, 2));

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
    }
}
