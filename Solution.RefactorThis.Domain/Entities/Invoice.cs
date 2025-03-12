using Solution.RefactorThis.Domain.Constants;
using Solution.RefactorThis.Domain.Enums;

namespace Solution.RefactorThis.Domain.Entities;

public record Invoice
{
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal TaxAmount { get; set; }
    public List<Payment> Payments { get; set; } = new();

    public InvoiceType Type { get; set; }

    public bool IsNoPaymentNeeded(out string message)
    {
        if (Amount != 0)
        {
            message = string.Empty;
            return false;
        }

        if (Payments is null || !Payments.Any())
        {
            message = ResponseMessage.NO_PAYMENT_NEEDED;
            return true;
        }

        throw new InvalidOperationException(InvalidOperationMessage.INVALID_INVOICE_STATE);
    }

    public bool IsPartialPaymentSuccess(Payment paymentRequest, out string message)
    {
        message = string.Empty;

        if (!Payments.Any()) return false;
        

        decimal paymentSum = Payments?.Sum(x => x.Amount) ?? 0;
        decimal remainingAmount = Amount - AmountPaid;

        if (paymentSum != 0 && Amount == paymentSum)
        {
            message = ResponseMessage.INVOICE_IS_FULLYPAID;
        }
        else if (paymentSum != 0 && paymentRequest.Amount > remainingAmount)
        {
            message = ResponseMessage.PAYMENT_GREATERTHAN_PARTIAL_REMAINING_AMOUNT;
        }
        else
        {
            bool isFinalPartialPayment = remainingAmount.Equals(paymentRequest.Amount);

            message = isFinalPartialPayment
                        ? ResponseMessage.PARTIAL_FINAL_PAYMENT_RECIEVED
                        : ResponseMessage.PARTIAL_PAYMENT_RECIEVED_WITH_BALANCE;

            AmountPaid += paymentRequest.Amount;
            Payments?.Add(paymentRequest);

            switch (Type)
            {
                case InvoiceType.Standard:
                    break;
                case InvoiceType.Commercial:
                    TaxAmount += paymentRequest.Amount * 0.14m;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return true;
    }
}




