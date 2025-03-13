using MediatR;
using Microsoft.Extensions.Logging;
using Solution.RefactorThis.Domain.DTOs;
using SolutionRefactorThis.Application.Commands.ProcessPayment;

namespace Solution.RefactorThis.Console.App
{
    public interface ITestService
    {
        void SayHello();
        Task TestExam();
    }

    public class TestService : ITestService
    {
        private readonly ILogger<TestService> _logger;
        private readonly ISender _sender;

        public TestService(ILogger<TestService> logger,
            ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        public void SayHello()
        {
            _logger.LogInformation("Hello, World!");
        }

        public async Task TestExam()
        {
            var response = await _sender.Send(new ProcessPaymentCommand(new ProcessPaymentDTO { }));

            _logger.LogInformation(response);
        }
    }
}
