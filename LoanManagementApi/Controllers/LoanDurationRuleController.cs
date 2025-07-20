using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanDurationRuleController : ControllerBase
    {
        private readonly ILoanDurationRuleService _loanDurationRuleService;

        public LoanDurationRuleController(ILoanDurationRuleService loanDurationRuleService)
        {
            _loanDurationRuleService = loanDurationRuleService;
        }


        [HttpGet]
        public async Task<ActionResult<GetAllDurationRulesResponseModel>> GetAllRules()
        {
            try
            {
                var result = await _loanDurationRuleService.GetAllAsync();

                if (!result.Status)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    Status = false,
                    Message = "An error occurred while retrieving rules."
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetLoanDurationRuleResponseModel>> GetRuleById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "ID cannot be empty."
                });
            }

            try
            {
                var result = await _loanDurationRuleService.GetByIdAsync(id);

                if (!result.Status)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "An error occurred while retrieving the rule."
                });
            }
        }


        [HttpGet("by-amount/{amount:decimal}")]
        public async Task<ActionResult<GetLoanDurationRuleResponseModel>> FindRuleByAmount(decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest(new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "Amount must be greater than zero."
                });
            }

            try
            {
                var result = await _loanDurationRuleService.FindByAmountAsync(amount);

                if (!result.Status)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "An error occurred while searching for the rule."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> CreateRule([FromBody] CreateRuleRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    Status = false,
                    Message = "Invalid request data."
                });
            }

            if (model.MinAmount >= model.MaxAmount)
            {
                return BadRequest(new BaseResponse
                {
                    Status = false,
                    Message = "Minimum amount must be less than maximum amount."
                });
            }

            if (model.MaxDurationInMonths <= 0)
            {
                return BadRequest(new BaseResponse
                {
                    Status = false,
                    Message = "Maximum duration must be greater than zero."
                });
            }

            try
            {
                var result = await _loanDurationRuleService.CreateRuleAsync(model);

                if (!result.Status)
                {
                    return UnprocessableEntity(result);
                }

                return CreatedAtAction(
                    nameof(GetRuleById),
                    new { id = "new" },
                    result
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    Status = false,
                    Message = "An error occurred while creating the rule."
                });
            }
        }
    }
}