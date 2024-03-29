﻿using Microsoft.AspNetCore.Mvc;
using sup_traders.Business.Models;
using sup_traders.Business.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace sup_traders.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

        [SwaggerOperation(Summary = "Register a random user at runtime.")]
        [HttpPost]
        public Return<User> RegisterUser()
        {
            return _userRepository.RegisterUser(new Business.Models.User { });
        }

        [SwaggerOperation(Summary = "User deposit.")]
        [HttpPost]
        public bool Deposit(Balance b)
        {
            return _userRepository.UpdateUserBalance(b.id, b.amount, OrgType.DEPOSIT);
        }

        [SwaggerOperation(Summary = "User withdraw.")]
        [HttpPost]
        public bool Withdraw(Balance b)
        {
            return _userRepository.UpdateUserBalance(b.id, b.amount, OrgType.WITHDRAW);
        }
    }
}
