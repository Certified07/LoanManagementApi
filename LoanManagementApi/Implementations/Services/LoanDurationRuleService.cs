using LoanManagementApi.DTOs;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Implementations.Services
{
    public class LoanDurationRuleService : ILoanDurationRuleService
    {
        private readonly ILoanDurationRuleRepository _loanDurationRuleRepository;

        public LoanDurationRuleService(ILoanDurationRuleRepository loanDurationRuleRepository)
        {
            _loanDurationRuleRepository = loanDurationRuleRepository;
        }
        public async Task<GetLoanDurationRuleResponseModel> FindByAmountAsync(decimal amount)
        {
            var rule = await _loanDurationRuleRepository.FindByAmountAsync(amount);
            if (rule == null)
            {
                return new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "No rule found for the specified amount."
                };
            }
            return new GetLoanDurationRuleResponseModel
            {
                Status = true,
                Data = new LoanDurationDTO
                {
                    Id = rule.Id,
                    MinAmount = rule.MinAmount,
                    MaxAmount = rule.MaxAmount,
                    MaxDurationInMonths = rule.MaxDurationInMonths
                }
            };
        }
        public async Task<GetLoanDurationRuleResponseModel> GetByIdAsync(string id)
        {
            var rule = await _loanDurationRuleRepository.GetByIdAsync(id);
            if (rule == null)
            {
                return new GetLoanDurationRuleResponseModel
                {
                    Status = false,
                    Message = "No rule found with the specified ID."
                };
            }
            return new GetLoanDurationRuleResponseModel
            {
                Status = true,
                Data = new LoanDurationDTO
                {
                    Id = rule.Id,
                    MinAmount = rule.MinAmount,
                    MaxAmount = rule.MaxAmount,
                    MaxDurationInMonths = rule.MaxDurationInMonths
                }
            };
        }
        public async Task<GetAllDurationRulesResponseModel> GetAllAsync()
        {
            var rules = await _loanDurationRuleRepository.GetAllAsync();
            if (rules == null || !rules.Any())
            {
                return new GetAllDurationRulesResponseModel
                {
                    Status = false,
                    Message = "No rules found."
                };
            }
            var data = rules.Select(r => new LoanDurationDTO
            {
                Id = r.Id,
                MinAmount = r.MinAmount,
                MaxAmount = r.MaxAmount,
                MaxDurationInMonths = r.MaxDurationInMonths
            }).ToList();
            return new GetAllDurationRulesResponseModel
            {
                Status = true,
                Message = "Rules retrieved successfully.",
                Data = data

            };
        }

        public async Task<BaseResponse> CreateRuleAsync(CreateRuleRequestModel model)
        {
            var check = await _loanDurationRuleRepository.IsOverlappingRuleAsync(model.MinAmount, model.MaxAmount);
            if (check)
                return new BaseResponse
                {
                    Message = "A rule with an overlapping amount range already exists.",
                    Status = false
                };

            var rule = new LoanDurationRule
            {
                MinAmount = model.MinAmount,
                MaxAmount = model.MaxAmount,
                MaxDurationInMonths = model.MaxDurationInMonths
            };

            await _loanDurationRuleRepository.AddAsync(rule);

            return new BaseResponse
            {
                Message = "Rule created successfully.",
                Status = true
            };
        }
    }
}
