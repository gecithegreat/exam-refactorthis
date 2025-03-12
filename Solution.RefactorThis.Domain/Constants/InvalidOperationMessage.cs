namespace Solution.RefactorThis.Domain.Constants;

public static class InvalidOperationMessage
{
    public static string MISMATCH_INVOICE_PAYMENT = "There is no invoice matching this payment";
    public static string INVALID_INVOICE_STATE = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
}

