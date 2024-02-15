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
            var s = _shareAccessor.GetShare(e.shareCode) ?? null;
            var u = _userAccessor.GetUser(e.userId) ?? null;

            bool shareCheck = false;

            if (s != null && u != null && s.price > 0)
            {
                var newBalance = u.CalculateNewBalance(u.balance, e.shareAmount, t, s.price);

                if (t == OrgType.SELL)
                {
                    shareCheck = e.shareAmount > 0 && e.shareAmount < s.baseCount;
                    e.shareAmount *= -1;
                }
                if (t == OrgType.BUY)
                {
                    shareCheck = e.shareAmount > 0 && e.shareAmount < s.count;
                }

                if (newBalance >= 0 && shareCheck)
                {
                    if (_exchangeAccessor.RegisterExchange(e))
                    {
                        if (_shareAccessor.UpdateShare(e.shareCode, e.shareAmount)) // Handle share amount, also updates share value using triggers.
                        {
                            if (_userAccessor.UpdateUserBalance(e.userId, newBalance)) // Upsert Exchange table.
                            {

                                return new Return<bool>()
                                {
                                    Data = true,
                                    Message = $"{e.userId}, {t}, {e.shareAmount}",
                                };
                            }
                        }
                    }
                    else
                    {
                        return new Return<bool>()
                        {
                            Data = false,
                            Message = "",
                        };
                    }
                }
                else
                {
                    return new Return<bool>()
                    {
                        Data = false,
                        Message = "Not have enough user balance or share amount.",
                    };
                }
            }

            return new Return<bool>()
            {
                Data = false,
                Message = "User or share do not exist.",
            };
        }
    }
}
