using Microsoft.AspNetCore.Mvc;
using sup_traders.Business.Models;
using sup_traders.Business.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExchangeController(ILogger<ExchangeController> logger, IExchangeRepository exchangeRepository) : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger = logger;
        private readonly IExchangeRepository _exchangeRepository = exchangeRepository;


        [SwaggerOperation(Summary = "Buy share.")]
        [HttpPost]
        public Return<bool> Buy([FromBody]Exchange e)
        {
            return _exchangeRepository.TradeExchange(e, OrgType.BUY);
        }


        [SwaggerOperation(Summary = "Sell share.")]
        [HttpPost]
        public Return<bool> Sell([FromBody]Exchange e)
        {
            return _exchangeRepository.TradeExchange(e, OrgType.SELL);
        }
    }
}
