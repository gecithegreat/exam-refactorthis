using Microsoft.Extensions.DependencyInjection;
using Solution.RefactorThis.Persistence.Repositories;
using SolutionRefactorThis.Application.Interfaces.Invoices;

namespace Solution.RefactorThis.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services) =>
          services
            .AddInvoiceRepository()
            .AddAnotherDependency();


        public static IServiceCollection AddInvoiceRepository(this IServiceCollection services)
        { 
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            return services;
        }

        // for example purpose only
        public static IServiceCollection AddAnotherDependency(this IServiceCollection services) { return services; }
    }
}
