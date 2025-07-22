using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LoanTypeController : ControllerBase
    {
        private readonly ILoanTypeService _loanTypeService;

        public LoanTypeController(ILoanTypeService loanTypeService)
        {
            _loanTypeService = loanTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _loanTypeService.GetAllAsync();

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
                    Message = "An error occurred while retrieving loan types",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid loan type ID",
                        Status = false
                    });
                }

                var result = await _loanTypeService.GetByIdAsync(id);

                if (result == null)
                {
                    return BadRequest(new
                    {
                        Message = "Loan type not found",
                        Status = false
                    });
                }

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
                    Message = "An error occurred while retrieving the loan type",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanTypeRequestModel model)
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

                var result = await _loanTypeService.CreateAsync(model);

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
                    Message = "An error occurred while creating the loan type",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanTypeRequestModel model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid loan type ID",
                        Status = false
                    });
                }

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

                var result = await _loanTypeService.UpdateAsync(id, model);

                if (result == null)
                {
                    return BadRequest(new
                    {
                        Message = "Loan type not found",
                        Status = false
                    });
                }

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
                    Message = "An error occurred while updating the loan type",
                    Status = false,
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid loan type ID",
                        Status = false
                    });
                }

                var result = await _loanTypeService.DeleteAsync(id);

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
                    Message = "An error occurred while deleting the loan type",
                    Status = false,
                    Error = ex.Message
                });
            }
        }
    }
}
