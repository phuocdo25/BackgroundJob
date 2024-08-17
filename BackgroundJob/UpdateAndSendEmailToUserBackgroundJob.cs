using BackgroundJob.Models;
using BackgroundJob.Services;

namespace BackgroundJob;

public class UpdateAndSendEmailToUserBackgroundJob : BackgroundService
{
    private readonly ILogger<UpdateAndSendEmailToUserBackgroundJob> _logger;
    private readonly IServiceScopeFactory _factory;

    public UpdateAndSendEmailToUserBackgroundJob(
        ILogger<UpdateAndSendEmailToUserBackgroundJob> logger,
        IServiceScopeFactory factory
    )
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await HandleAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task HandleAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        var userService = asyncScope.ServiceProvider.GetRequiredService<IUserService>();

        var users = await userService.UpdatePasswordAsync(stoppingToken);

        if (users.Any())
        {
            var emailService = asyncScope.ServiceProvider.GetRequiredService<IEmailService>();
            users.ToList().ForEach(async x =>
                await emailService.SendEmailAsync(x.Email, "body", stoppingToken));
        }
    }
}
