using System;
using System.Threading.Tasks;
using Order.Application.Models;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Infrastructure;

namespace Order.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings settings;
        private readonly ILogger<EmailService> logger;

        public EmailService(EmailSettings settings, ILogger<EmailService> logger)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
            logger.LogInformation("E-Mail sent successfully.");
            await Task.CompletedTask;

            return true;
        }
    }
}