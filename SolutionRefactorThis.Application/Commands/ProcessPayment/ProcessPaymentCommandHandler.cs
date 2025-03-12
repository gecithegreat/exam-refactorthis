using AutoMapper;
using FluentValidation;
using MediatR;
using Solution.RefactorThis.Domain.Constants;
using Solution.RefactorThis.Domain.Entities;
using SolutionRefactorThis.Application.Interfaces.Invoices;

namespace SolutionRefactorThis.Application.Commands.ProcessPayment
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, string>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProcessPaymentCommand> _validator;

        public ProcessPaymentCommandHandler(IInvoiceRepository invoiceRepository,
            IMapper mapper,
            IValidator<ProcessPaymentCommand> validator)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<string> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Commented to match the unit test, left it as a reference
                //throw new ValidationException(validationResult.Errors);

                throw new InvalidOperationException(InvalidOperationMessage.MISMATCH_INVOICE_PAYMENT);
            }

            var paymentRequest = _mapper.Map<Payment>(request.Payment);

            var invoiceData = await _invoiceRepository.GetInvoiceAsync(paymentRequest.Reference);

            if (invoiceData is null) throw new InvalidOperationException(InvalidOperationMessage.MISMATCH_INVOICE_PAYMENT);
            if (invoiceData.IsNoPaymentNeeded(out var requiredPaymentResponse)) return requiredPaymentResponse;
            if (invoiceData.IsPartialPaymentSuccess(paymentRequest, out var partialPaymentResponse)) return partialPaymentResponse;

            ProcessNewPayment(paymentRequest, invoiceData, out var newPaymentResponse);

            await _invoiceRepository.SaveInvoiceAsync(invoiceData);

            return newPaymentResponse;
        }

        private void ProcessNewPayment(Payment paymentRequest, Invoice? invoiceData, out string message)
        {
            message = ResponseMessage.PAYMENT_GREATERTHAN_INVOICE_AMOUNT;

            if (paymentRequest.Amount > invoiceData?.Amount)
                return;


            bool isPaidOnFullAmount = invoiceData?.Amount == paymentRequest.Amount;

            message = isPaidOnFullAmount
                        ? ResponseMessage.INVOICE_FULLY_PAID
                        : ResponseMessage.INVOICE_PARTIALLY_PAID;

            invoiceData.AmountPaid = paymentRequest.Amount;
            invoiceData.TaxAmount = paymentRequest.Amount * 0.14m;
            invoiceData.Payments.Add(paymentRequest);
        }
    }
}
