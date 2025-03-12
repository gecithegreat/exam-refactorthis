using AutoMapper;
using Solution.RefactorThis.Domain.DTOs;
using Solution.RefactorThis.Domain.Entities;

namespace SolutionRefactorThis.Application.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<ProcessPaymentDTO, Payment>();
        }
    }
}
