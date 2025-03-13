using Solution.RefactorThis.Domain.Entities;

namespace SolutionRefactorThis.Application.Interfaces.Invoices
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceAsync(string reference);
        Task SaveInvoiceAsync(Invoice invoice);
        Task AddAsync(Invoice invoice);
    }
}
