using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Apply([FromBody] LoanRequestModel model)
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

                var result = await _loanService.ApplyAsync(model);

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
                    Message = "An error occurred while applying for the loan",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost("approve/{loanId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(string loanId)
        {
            try
            {
                if (string.IsNullOrEmpty(loanId))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid loan ID",
                        Status = false
                    });
                }

                var result = await _loanService.ApproveAsync(loanId);

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
                    Message = "An error occurred while approving the loan",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost("reject/{loanId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(string loanId)
        {
            try
            {
                if (string.IsNullOrEmpty(loanId))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid loan ID",
                        Status = false
                    });
                }

                var result = await _loanService.RejectAsync(loanId);

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
                    Message = "An error occurred while rejecting the loan",
                    Status = false,
                    Error = ex.Message
                });
            }
        }
    }
}