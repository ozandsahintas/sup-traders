using Dapper;
using sup_traders.Access;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Business.Repositories
{
    public interface IExchangeRepository
    {
        public Return<Share> RegisterShare(Share s);
        public Return<List<Share>> LoadShares();
    }

    public class ExchangeRepository : IExchangeRepository
    {
        private readonly GameManager _gm;
        private readonly IExchangeAccesor _exchangeAccessor;

        public ExchangeRepository(GameManager gm, IExchangeAccesor exchangeAccesor)
        {
            _gm = gm;
            _exchangeAccessor = exchangeAccesor;
        }

        public Return<Share> RegisterShare(Share s)
        {
            if (_gm.RegisterShare(s))
            {
                if (_exchangeAccessor.RegisterShare(s))
                {
                    return new Return<Share>()
                    {
                        Data = s,
                        Message = "Share created successfully!",
                    };
                } else
                {
                    return new Return<Share>()
                    {
                        Data = s,
                        Message = "Share code cannot be added! \n Possible reasons: code limit(3)",
                    };
                }
            } else
            {
                return new Return<Share>()
                {
                    Data = s,
                    Message = "Share code exists!",
                };
            }
        }
        public Return<List<Share>> LoadShares()
        {
            var msg = string.Empty;
            var shares = _exchangeAccessor.LoadShares();

            if (shares == null)
            {
                msg = "There is no share registered!";
            } else
            {
                foreach (var s in shares)
                {
                    _gm.RegisterShare(s);
                }
            }

            return new Return<List<Share>>()
            {
                Message = msg,
                Data =  shares,
            };
        }
    }
}
