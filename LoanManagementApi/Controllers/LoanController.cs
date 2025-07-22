using LoanManagementApi.Implementations.Services;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IRepaymentService _repaymentService;
        public LoanController(ILoanService loanService, IRepaymentService repaymentService)
        {
            _loanService = loanService;
            _repaymentService = repaymentService;
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

        [HttpGet("defaults")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDefaultedLoans()
        {
            var result = await _loanService.GetDefaultedLoans();
            return Ok(result);
        }

        [HttpGet("paid")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaidLoans()
        {
            var result = await _loanService.GetPaidLoans();
            return Ok(result);
        }

        [HttpGet("outstanding")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOutstandingLoans()
        {
            var result = await _loanService.GetOutstandingLoans();
            return Ok(result);
        }
        [HttpPost("pay")]
        public async Task<IActionResult> MakeRepayment([FromBody] MakeRepaymentRequestModel request)
        {
            var result = await _repaymentService.MakePaymentAsync(request);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
