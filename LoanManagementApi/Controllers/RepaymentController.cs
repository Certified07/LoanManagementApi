using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/repayments")]
    public class RepaymentController : ControllerBase
    {
        private readonly IRepaymentService _repaymentService;

        public RepaymentController(IRepaymentService repaymentService)
        {
            _repaymentService = repaymentService;
        }

        [HttpPost("pay")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MakeRepayment([FromBody] MakeRepaymentRequestModel request)
        {
            var result = await _repaymentService.MakePaymentAsync(request);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet("schedule/{loanId}")]
        [Authorize]
        public async Task<IActionResult> GetRepaymentSchedule(string loanId)
        {
            var result = await _repaymentService.GetRepaymentSummaryAsync(loanId);
            return result == null ? NotFound(new { Message = "No repayment schedule found", Status = false }) : Ok(result);
        }

        [HttpGet("history/{loanId}")]
        [Authorize]
        public async Task<IActionResult> GetRepaymentHistory(string loanId)
        {
            var result = await _repaymentService.GetHistoryByLoanIdAsync(loanId);
            return result == null || !result.Any() ? NotFound(new { Message = "No repayment history found", Status = false }) : Ok(result);
        }
    }
}

