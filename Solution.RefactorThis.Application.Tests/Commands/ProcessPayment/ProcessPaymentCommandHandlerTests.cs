using AutoMapper;
using FluentValidation;
using Moq;
using Solution.RefactorThis.Domain.DTOs;
using SolutionRefactorThis.Application.Commands.ProcessPayment;
using SolutionRefactorThis.Application.Interfaces.Invoices;
using FluentValidation.Results;
using Solution.RefactorThis.Domain.Entities;

namespace Solution.RefactorThis.Application.Tests.Commands.ProcessPayment
{
    [TestFixture]
    public class ProcessPaymentCommandHandlerTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<ProcessPaymentCommand>> _validatorMock;

        public ProcessPaymentCommandHandlerTests()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<ProcessPaymentCommand>>();
        }

        [Test]
        public async Task ProcessPayment_Should_ThrowException_When_NoInoiceFoundForPaymentReference()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { });

            Invoice invoice = null;

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                            .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
               _invoiceRepositoryMock.Object,
               _mapperMock.Object,
               _validatorMock.Object
            );

            string failureMessage = string.Empty;
            try
            {
                var result = await handler.Handle(command, CancellationToken.None);
            }
            catch (InvalidOperationException e)
            {
                failureMessage = e.Message;
            }

            Assert.AreEqual("There is no invoice matching this payment", failureMessage);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { });

            var invoice = new Invoice()
            {
                Amount = 0,
                AmountPaid = 0,
                Payments = null
            };

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                            .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
               _invoiceRepositoryMock.Object,
               _mapperMock.Object,
               _validatorMock.Object
            );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("no payment needed", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { });
            
            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 10,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 10
                    }
                }
            };

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                          .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
               _invoiceRepositoryMock.Object,
               _mapperMock.Object,
               _validatorMock.Object
            );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("invoice was already fully paid", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 6 });

            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                       .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("the payment is greater than the partial amount remaining", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 6 });

            var invoice = new Invoice()
            {
                Amount = 5,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                   .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("the payment is greater than the invoice amount", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 5});

            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };


            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                   .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("final partial payment received, invoice is now fully paid", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 10 });

            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>() { new Payment() { Amount = 10 } }
            };


            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                   .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("invoice was already fully paid", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 1 });

            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };


            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                   .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("another partial payment received, still not fully paid", result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
        {
            var command = new ProcessPaymentCommand(new ProcessPaymentDTO { Amount = 1 });

            var invoice = new Invoice()
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };

            _invoiceRepositoryMock.Setup(r => r.AddAsync(invoice))
                   .Returns(Task.CompletedTask);

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceAsync(command.Payment.Reference))
                             .ReturnsAsync(invoice);

            var mappedPayment = new Payment
            {
                Amount = command.Payment.Amount,
                Reference = command.Payment.Reference,
            };

            _mapperMock.Setup(m => m.Map<Payment>(command.Payment))
                       .Returns(mappedPayment);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());


            var handler = new ProcessPaymentCommandHandler(
                _invoiceRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
             );

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("invoice is now partially paid", result);
        }
    }
}
