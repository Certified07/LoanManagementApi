using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Status = false,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                var result = await _userService.RegisterAsync(model);

                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "An error occurred during registration",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Status = false,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                var result = await _userService.LoginUser(model);

                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "An error occurred during login",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

       
    }
}
