using MediatR;
using Solution.RefactorThis.Domain.DTOs;

namespace SolutionRefactorThis.Application.Commands.ProcessPayment
{
    public sealed record ProcessPaymentCommand(ProcessPaymentDTO Payment) : IRequest<string>;
}
