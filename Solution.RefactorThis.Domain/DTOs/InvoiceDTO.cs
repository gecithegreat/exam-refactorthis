using Solution.RefactorThis.Domain.Entities;
using Solution.RefactorThis.Domain.Enums;

namespace Solution.RefactorThis.Domain.DTOs;
public record InvoiceDTO
{
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal TaxAmount { get; set; }
    public List<Payment> Payments { get; set; }

    public InvoiceType Type { get; set; }
}

