using AutoMapper;
using Solution.RefactorThis.Domain.DTOs;
using Solution.RefactorThis.Domain.Entities;

namespace SolutionRefactorThis.Application.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile() 
        {
            CreateMap<InvoiceDTO, Invoice>();
        }
    }
}
