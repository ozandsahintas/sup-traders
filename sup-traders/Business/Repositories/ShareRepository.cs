using Dapper;
using Microsoft.AspNetCore.Mvc;
using sup_traders.Access;
using sup_traders.Business.Helpers;
using sup_traders.Business.Models;

namespace sup_traders.Business.Repositories
{
    public interface IShareRepository
    {
        public Return<Share> RegisterShare(Share s);
        public Return<List<Share>> LoadShares();
        public decimal GetShareValue(string code);
        public bool UpdateShare(string code, decimal amount);
    }

    public class ShareRepository(GameManager gm, IShareAccessor shareAccessor) : IShareRepository
    {
        private readonly GameManager _gm = gm;
        private readonly IShareAccessor _shareAccessor = shareAccessor;


        public Return<Share> RegisterShare(Share s)
        {
            if (_gm.RegisterShare(s))
            {
                if (_shareAccessor.RegisterShare(s))
                {
                    return new Return<Share>()
                    {
                        Data = s,
                        Message = "Share created successfully!",
                    };
                }
                else
                {
                    return new Return<Share>()
                    {
                        Data = s,
                        Message = "Share code cannot be added! \n Possible reasons: code limit(3)",
                    };
                }
            }
            else
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
            var shares = _shareAccessor.LoadShares();

            if (shares == null)
            {
                msg = "There is no share registered!";
            }
            else
            {
                foreach (var s in shares)
                {
                    _gm.RegisterShare(s);
                }
            }

            return new Return<List<Share>>()
            {
                Message = msg,
                Data = shares,
            };
        }
        public decimal GetShareValue(string code)
        {
            return _shareAccessor.GetShareValue(code);
        }
        public bool UpdateShare(string code, decimal amount)
        {
            return _shareAccessor.UpdateShare(code, amount);
        }
    }

}
