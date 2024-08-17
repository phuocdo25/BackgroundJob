namespace BackgroundJob.Services;
public interface IEmailService
{
    Task SendEmailAsync(string mailTo, string body, CancellationToken cancellationToken);
}
