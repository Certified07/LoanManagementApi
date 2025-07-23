using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid input data",
                    Status = false,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _userService.RegisterAsync(model);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid input data",
                    Status = false,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _userService.LoginUser(model);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
