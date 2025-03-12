using Solution.RefactorThis.Domain.Entities;
using SolutionRefactorThis.Application.Interfaces.Invoices;

namespace Solution.RefactorThis.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private Invoice _invoice; 

        public InvoiceRepository()
        {
            
        }

        public async Task AddAsync(Invoice invoice)
        {
            _invoice = invoice;
        }

        public async Task<Invoice> GetInvoiceAsync(string reference)
        {
            return _invoice;
        }

        public async Task SaveInvoiceAsync(Invoice invoice)
        {
            //saves the invoice to the database
        }
    }
}
