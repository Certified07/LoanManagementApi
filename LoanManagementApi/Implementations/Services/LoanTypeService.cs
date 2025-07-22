using LoanManagementApi.DTOs;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Implementations.Services
{
    public class LoanTypeService : ILoanTypeService
    {
        private readonly ILoanTypeRepository _loanTypeRepository;

        public LoanTypeService(ILoanTypeRepository loanTypeRepository)
        {
            _loanTypeRepository = loanTypeRepository;
        }
        public async Task<GetAllLoanTypesResponseModel> GetAllAsync()
        {
            var loanTypes = await _loanTypeRepository.GetAllAsync();

            return new GetAllLoanTypesResponseModel
            {
                Data = loanTypes.Select(lt => new LoanTypeDTO
                {
                    Id = lt.Id,
                    Name = lt.Name,
                    MaxAmount = lt.MaxAmount,
                    MaxDurationInMonths = lt.MaxDurationInMonths
                }).ToList(),
                Message = "Sucessfully returned",
                Status = true
            };
        }
        public async Task<GetLoanTypeResponseModel?> GetByIdAsync(int id)
        {

            var loanType = await _loanTypeRepository.GetByIdAsync(id);
            if (loanType == null) return null;

            return new GetLoanTypeResponseModel
            {
                Data = new LoanTypeDTO
                {
                    Id = loanType.Id,
                    Name = loanType.Name,
                    MaxAmount = loanType.MaxAmount,
                    MaxDurationInMonths = loanType.MaxDurationInMonths
                },
                Message = "Successfully returned",
                Status = true
            };
        }
        public async Task<GetLoanTypeResponseModel> CreateAsync(LoanTypeRequestModel request)
        {
            var check = await _loanTypeRepository.GetByNameAsync(request.Name);
            if (check != null)
            {
                return new GetLoanTypeResponseModel
                {
                    Message = "Loan type already exists",
                    Status = false
                };
            }
            if (!Enum.TryParse<RepaymentType>(request.RepaymentType, true, out _))
            {
                return new GetLoanTypeResponseModel
                {
                    Message = "Repayment type not available",
                    Status = false
                };
            }

            var loanType = new LoanType
            {
                Name = request.Name,
                MaxAmount = request.MaxAmount,
                MaxDurationInMonths = request.MaxDurationInMonths,
                RepaymentType = request.RepaymentType.ToLower() == RepaymentType.Flexible.ToString().ToLower() ? RepaymentType.Flexible : RepaymentType.Fixed
            };

            await _loanTypeRepository.AddAsync(loanType);

            return new GetLoanTypeResponseModel
            {
                Data = new LoanTypeDTO
                {
                    Id = loanType.Id,
                    Name = loanType.Name,
                    MaxAmount = loanType.MaxAmount,
                    MaxDurationInMonths = loanType.MaxDurationInMonths,
                    
                },
                Message = "Loan type created successfully",
                Status = true
            };
        }
        public async Task<GetLoanTypeResponseModel?> UpdateAsync(int id, LoanTypeRequestModel request)
        {
            var existing = await _loanTypeRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = request.Name;
            existing.MaxAmount = request.MaxAmount;
            existing.MaxDurationInMonths = request.MaxDurationInMonths;

            await _loanTypeRepository.UpdateAsync(existing);

            return new GetLoanTypeResponseModel
            {
                Data = new LoanTypeDTO
                {
                    Id = existing.Id,
                    Name = existing.Name,
                    MaxAmount = existing.MaxAmount,
                    MaxDurationInMonths = existing.MaxDurationInMonths
                },
                Message = "Sucessfully updated",
                Status = true
            };
        }
        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var loanType = await _loanTypeRepository.GetByIdAsync(id);
            if (loanType == null)
                return new BaseResponse
                {
                    Message = "Loan type not found",
                    Status = false

                };

            await _loanTypeRepository.DeleteAsync(loanType);
            return new BaseResponse
            {
                Message = "Sucessfully deleted",
                Status = true
            };
        }

    }
}
