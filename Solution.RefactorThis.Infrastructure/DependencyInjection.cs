using Microsoft.Extensions.DependencyInjection;
using Solution.RefactorThis.Persistence;

namespace Solution.RefactorThis.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddPersistence();
    }
}
