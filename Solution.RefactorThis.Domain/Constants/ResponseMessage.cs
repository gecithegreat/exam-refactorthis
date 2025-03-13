namespace Solution.RefactorThis.Domain.Constants;

public static class ResponseMessage
{
    public static string NO_PAYMENT_NEEDED = "no payment needed";
    public static string INVOICE_IS_FULLYPAID = "invoice was already fully paid";
   
    public static string PARTIAL_FINAL_PAYMENT_RECIEVED = "final partial payment received, invoice is now fully paid";
    public static string PARTIAL_PAYMENT_RECIEVED_WITH_BALANCE = "another partial payment received, still not fully paid";

    public static string PAYMENT_GREATERTHAN_PARTIAL_REMAINING_AMOUNT = "the payment is greater than the partial amount remaining";
    public static string PAYMENT_GREATERTHAN_INVOICE_AMOUNT = "the payment is greater than the invoice amount";

    public static string INVOICE_FULLY_PAID = "invoice is now fully paid";
    public static string INVOICE_PARTIALLY_PAID = "invoice is now partially paid";
}

