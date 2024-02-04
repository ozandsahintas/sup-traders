using Microsoft.AspNetCore.Mvc;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExchangeController(ILogger<ExchangeController> logger) : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger = logger;
        private static readonly GameManager gm = GameManager.GetInstance();

        [HttpPost]
        public void Buy()
        {
            _logger.Log(LogLevel.Information, gm.Test());
            return;
        }


        [HttpPost]
        public void Sell()
        {
            _logger.Log(LogLevel.Information, "Sell!");
            return;
        }
    }
}
