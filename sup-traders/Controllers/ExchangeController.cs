using Microsoft.AspNetCore.Mvc;
using sup_traders.Business.Models;
using sup_traders.Business.Repositories;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExchangeController(ILogger<ExchangeController> logger, IExchangeRepository exchangeRepository) : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger = logger;
        private readonly IExchangeRepository _exchangeRepository = exchangeRepository;

        [HttpPost]
        public void Buy()
        {
            return;
        }

        [HttpPost]
        public void Sell()
        {
            return;
        }

        [HttpGet]
        public Return<List<Share>> LoadShares()
        {
            return _exchangeRepository.LoadShares();
        }

        [HttpPost]
        public Return<Share> RegisterShare([FromBody] Share s)
        {
            return _exchangeRepository.RegisterShare(s);
        }
    }
}
