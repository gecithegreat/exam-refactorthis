using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solution.RefactorThis.Console.App;
using Solution.RefactorThis.Infrastructure;
using SolutionRefactorThis.Application;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        // Register dependencies
        services.AddSingleton<ITestService, TestService>();
        services.AddApplication();
        services.AddInfrastructure();
    })
    .Build();

var testService = host.Services.GetRequiredService<ITestService>();

testService.SayHello(); 
await testService.TestExam();
