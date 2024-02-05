using Microsoft.AspNetCore.Mvc;
using sup_traders.Business.Models;
using sup_traders.Business.Repositories;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost]
        public Return<User> RegisterUser()
        {
            return _userRepository.RegisterUser(new Business.Models.User { });
        }

        [HttpPost]
        public Return<bool> Deposit(Balance b)
        {
            return _userRepository.UpdateUserBalance(b.id, b.amount);

        }
    }
}
