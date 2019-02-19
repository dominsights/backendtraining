using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTraining.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendTraining.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody]User credentials)
        {
            var userPayload = _authService.LoginAsync(credentials.UserName, credentials.Password);

            if(userPayload == null)
            {
                return BadRequest("Invalid credentials.");
            }

            return Ok(userPayload);
        }

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
    }
}