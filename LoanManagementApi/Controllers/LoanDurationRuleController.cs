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

        /// <summary>
        /// Retrieves all loan duration rules
        /// </summary>
        /// <returns>A list of all loan duration rules</returns>
        /// <response code="200">Returns the list of rules</response>
        /// <response code="404">No rules found</response>
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

        /// <summary>
        /// Retrieves a loan duration rule by ID
        /// </summary>
        /// <param name="id">The unique identifier of the rule</param>
        /// <returns>The loan duration rule with the specified ID</returns>
        /// <response code="200">Returns the rule</response>
        /// <response code="404">Rule not found</response>
        /// <response code="400">Invalid ID format</response>
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

        /// <summary>
        /// Finds a loan duration rule by loan amount
        /// </summary>
        /// <param name="amount">The loan amount to search for</param>
        /// <returns>The applicable loan duration rule for the specified amount</returns>
        /// <response code="200">Returns the applicable rule</response>
        /// <response code="404">No rule found for the specified amount</response>
        /// <response code="400">Invalid amount</response>
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

        /// <summary>
        /// Creates a new loan duration rule
        /// </summary>
        /// <param name="model">The rule creation request containing min/max amounts and duration</param>
        /// <returns>Success or failure response</returns>
        /// <response code="201">Rule created successfully</response>
        /// <response code="400">Invalid request data or overlapping rule exists</response>
        /// <response code="422">Business rule violation</response>
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
                    new { id = "new" }, // You might want to return the actual ID if your service returns it
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