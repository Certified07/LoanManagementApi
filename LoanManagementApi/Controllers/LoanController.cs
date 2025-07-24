using LoanManagementApi.Implementations.Services;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILoanStatusTrackingService _loanStatusTrackingService;
        private readonly IRepaymentService _repaymentService;

        public LoanController(ILoanService loanService, ILoanStatusTrackingService loanStatusTrackingService, IRepaymentService repaymentService)
        {
            _loanService = loanService;
            _loanStatusTrackingService = loanStatusTrackingService;
            _repaymentService = repaymentService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Apply([FromBody] LoanRequestModel model)
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

            var result = await _loanService.ApplyAsync(model);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{loanId}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(string loanId)
        {
            var result = await _loanService.ApproveAsync(loanId);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{loanId}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(string loanId)
        {
            var result = await _loanService.RejectAsync(loanId);
            return result.Status ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLoanStatus(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId))
            {
                return BadRequest(new { Message = "Loan ID is required", Status = false });
            }

            try
            {
                var result = await _loanStatusTrackingService.GetLoanStatusAsync(loanId);

                if (!result.Status)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving loan status", Status = false });
            }
        }

        [HttpGet("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLoansByStatus([FromQuery] string status)
        {
            var response = await _loanService.GetLoanDetailsForCurrentUser();
            var result = response.Data.Where(x => x.Status == status).ToList();
            if (result == null)
                return BadRequest("Invalid loan status.");

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetLoanDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var loans = await _loanService.GetLoanDetailsForCurrentUser();
            return Ok(loans);
        }
        [HttpPost("payment")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MakeRepayment([FromBody] MakeRepaymentRequestModel request)
        {
            var result = await _repaymentService.MakePaymentAsync(request);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}