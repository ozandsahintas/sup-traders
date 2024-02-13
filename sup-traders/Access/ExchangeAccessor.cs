using Dapper;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Access
{
    public interface IExchangeAccesor
    {
        public bool RegisterExchange(Exchange e);
    }

    public class ExchangeAccesor(ConnectionHelper connectionHelper) : IExchangeAccesor
    {
        private readonly ConnectionHelper _connectionHelper = connectionHelper;

        public bool RegisterExchange(Exchange e)
        {
            var query = "DECLARE @sa int " +
                        "SELECT @sa = [shareAmount] FROM Exchanges WHERE shareCode = @sc AND userId = @uid " +
                        "IF(@sa IS NOT NULL) BEGIN " +
                            "IF(@sa + @amount = 0) BEGIN " +
                                "DELETE FROM Exchanges WHERE shareCode = @sc AND userId = @uid " +
                            "END " +
                            "ELSE BEGIN " +
                                "UPDATE Exchanges " +
                                "SET shareAmount = shareAmount + @amount " +
                                "WHERE shareCode = @sc AND userId = @uid " +
                            "END " +
                        "END " +
                        "ELSE BEGIN " +
                            "INSERT INTO Exchanges (shareCode, userId, shareAmount) " +
                            "VALUES (@sc , @uid, @amount) " +
                        "END";


            var parameters = new DynamicParameters();
            parameters.Add("sc", e.shareCode);
            parameters.Add("uid", e.userId);
            parameters.Add("amount", e.shareAmount);

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
    }
}
