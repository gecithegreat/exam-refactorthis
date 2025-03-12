namespace Solution.RefactorThis.Domain.DTOs;

public record ProcessPaymentDTO
{
    public decimal Amount { get; set; }
    public string Reference { get; set; }
}

