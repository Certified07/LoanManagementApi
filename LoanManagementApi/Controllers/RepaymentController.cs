
using LoanManagementApi.Implementations.Services;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.ResponseModel;
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

   
        [HttpGet("schedule/{loanId}")]
        public async Task<ActionResult<RepaymentScheduleResponseModel>> GetRepaymentSchedule(string loanId)
        {
            var result = await _repaymentService.GetRepaymentSummaryAsync(loanId);

            if (result == null)
            {
                return NotFound(new BaseResponse
                {
                    Message = "No repayment schedule found for the specified loan",
                    Status = false
                });
            }

            return Ok(result);
        }


        [HttpGet("history/{loanId}")]
        public async Task<ActionResult<List<RepaymentResponseModel>>> GetRepaymentHistory(string loanId)
        {
            var repayments = await _repaymentService.GetHistoryByLoanIdAsync(loanId);

            if (repayments == null || !repayments.Any())
            {
                return NotFound(new BaseResponse
                {
                    Message = "No repayment history found for the specified loan",
                    Status = false
                });
            }

            return Ok(repayments);
        }
    }
}
