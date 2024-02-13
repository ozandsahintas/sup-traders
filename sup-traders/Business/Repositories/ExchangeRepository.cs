using Dapper;
using sup_traders.Access;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Business.Repositories
{
    public interface IExchangeRepository
    {
        public Return<bool> TradeExchange(Exchange e, OrgType t);
    }

    public class ExchangeRepository(IExchangeAccesor exchangeAccessor,
                                    IShareAccessor shareAccessor,
                                    IUserAccessor userAccessor) : IExchangeRepository
    {
        private readonly IExchangeAccesor _exchangeAccessor = exchangeAccessor;
        private readonly IShareAccessor _shareAccessor = shareAccessor;
        private readonly IUserAccessor _userAccessor = userAccessor;

        public Return<bool> TradeExchange(Exchange e, OrgType t)
        {

            var sValue = _shareAccessor.GetShareValue(e.shareCode);

            if (sValue > 0)
            {
                if (_userAccessor.UpdateUserBalance(e.userId, e.shareAmount, t, sValue)) // Checks if user has enough balance and BUY.
                {
                    if (t == OrgType.SELL) { e.shareAmount *= -1; } // If we SELL share, we need to negate our share count.

                    if (_shareAccessor.UpdateShare(e.shareCode, e.shareAmount)) // Handle share amount, also updates share value using triggers.
                    {
                        if (_exchangeAccessor.RegisterExchange(e)) // Upsert Exchange table.
                        {
                            return new Return<bool>()
                            {
                                Data = true,
                                Message = $"{e.userId}, {t}, {e.shareAmount}",
                            };
                        }
                    }
                }
            }

            return new Return<bool>()
            {
                Data = false,
                Message = "",
            };
        }
    }
}
