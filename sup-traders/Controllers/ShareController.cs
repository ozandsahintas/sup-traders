using Microsoft.AspNetCore.Mvc;
using sup_traders.Business.Models;
using sup_traders.Business.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShareController(ILogger<ShareController> logger, IShareRepository shareRepository) : ControllerBase
    {
        private readonly ILogger<ShareController> _logger = logger;
        private readonly IShareRepository _shareRepository = shareRepository;


        [SwaggerOperation(Summary = "Load shares from DB.")]
        [HttpGet]
        public Return<List<Share>> LoadShares()
        {
            return _shareRepository.LoadShares();
        }


        [SwaggerOperation(Summary = "Register a share")]
        [HttpPost]
        public Return<Share> RegisterShare([FromBody] Share s)
        {
            return _shareRepository.RegisterShare(s);
        }
    }
}
