namespace BackgroundJob.Services;
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        ILogger<EmailService> logger
        )
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string mailTo, string body, CancellationToken cancellationToken)
    {
        // Send email logic here
        _logger.LogInformation("Send to: {mailTo}, message: {body}", mailTo, body);
        return Task.CompletedTask;
    }
}
