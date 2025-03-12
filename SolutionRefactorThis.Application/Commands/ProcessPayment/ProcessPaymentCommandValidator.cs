using FluentValidation;
using Solution.RefactorThis.Domain.Constants;

namespace SolutionRefactorThis.Application.Commands.ProcessPayment
{
    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        public ProcessPaymentCommandValidator()
        {
            RuleFor(x => x.Payment.Reference)
                .NotNull().WithMessage(InvalidOperationMessage.MISMATCH_INVOICE_PAYMENT);

            RuleFor(x => x.Payment.Reference)
               .NotEmpty().WithMessage(InvalidOperationMessage.MISMATCH_INVOICE_PAYMENT);
        }
    }
}
