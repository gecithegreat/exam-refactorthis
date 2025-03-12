namespace Solution.RefactorThis.Domain.Entities;

public record Payment
{
    public decimal Amount { get; set; }
    public string Reference { get; set; }
}

