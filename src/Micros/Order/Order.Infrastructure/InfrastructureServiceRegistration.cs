using Order.Application.Models;
using Order.Infrastructure.Mail;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Order.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Contracts.Infrastructure;

namespace Order.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(cfg.GetConnectionString("OrderingConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(c => cfg.GetSection("EmailSettings"));

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}