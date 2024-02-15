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
            bool result = false;

            var query = "DECLARE @sa int " +
                        "SELECT @sa = [shareAmount] FROM Exchanges WHERE shareCode = @sc AND userId = @uid " +

                        "IF(@sa IS NULL AND @amount > 0) BEGIN " +
                            "INSERT INTO Exchanges (shareCode, userId, shareAmount) VALUES (@sc , @uid, @amount) " +
                            "SELECT CAST(1 AS BIT) " +
                        "END " +
                        "ELSE IF(@sa IS NOT NULL) BEGIN " +
                            "IF(@sa + @amount = 0) BEGIN " +
                                "DELETE FROM Exchanges WHERE shareCode = @sc AND userId = @uid " +
                                "SELECT CAST(1 AS BIT) " +
                            "END " +
                            "ELSE IF(@sa + @amount > 0) BEGIN " +
                                "UPDATE Exchanges SET shareAmount = shareAmount + @amount WHERE shareCode = @sc AND userId = @uid " +
                                "SELECT CAST(1 AS BIT) " +
                            "END " +
                            "ELSE BEGIN " +
                                "SELECT CAST(0 AS BIT) " +
                            "END " +
                        "END ";


            var parameters = new DynamicParameters();
            parameters.Add("sc", e.shareCode);
            parameters.Add("uid", e.userId);
            parameters.Add("amount", e.shareAmount);

            using var connection = _connectionHelper.CreateSqlConnection();
            try
            {
                result = connection.ExecuteScalar<bool>(query, parameters);
            }
            catch (Exception)
            {
                return result;
            }

            return result;

        }
    }
}
