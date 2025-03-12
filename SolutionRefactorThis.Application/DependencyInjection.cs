using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SolutionRefactorThis.Application.Commands.ProcessPayment;
using SolutionRefactorThis.Application.Profiles;
using System.Reflection;

namespace SolutionRefactorThis.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
                .AddAutoMapper()
                .AddValidators();


        public static IServiceCollection AddAutoMapper(this IServiceCollection services) 
        {
            // Register AutoMapper in DI
            services.AddAutoMapper(typeof(PaymentProfile));
            services.AddAutoMapper(typeof(InvoiceProfile));
            return services; 
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProcessPaymentCommand>();
            return services;
        }
    }
}
