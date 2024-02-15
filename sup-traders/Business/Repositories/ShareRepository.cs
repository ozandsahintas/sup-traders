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
        public Share? GetShare(string code);
        public bool UpdateShare(string code, decimal amount);
    }

    public class ShareRepository(IShareAccessor shareAccessor) : IShareRepository
    {
        private readonly IShareAccessor _shareAccessor = shareAccessor;


        public Return<Share> RegisterShare(Share s)
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
                    Message = "Share code cannot be added! \n Possible reasons: code limit(3), code exists.",
                };
            }
        }
        public Share? GetShare(string code)
        {
            return _shareAccessor.GetShare(code);
        }
        public bool UpdateShare(string code, decimal amount)
        {
            return _shareAccessor.UpdateShare(code, amount);
        }
    }

}
