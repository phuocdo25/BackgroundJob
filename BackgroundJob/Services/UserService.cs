using BackgroundJob.Data;
using BackgroundJob.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJob.Services;
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ApplicationDbContext _dbContext;

    public UserService(
        ILogger<UserService> logger,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> UpdatePasswordAsync(CancellationToken cancellationToken)
    {
        var changePwStatus = "REQUIRE_CHANGE_PWD";
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var users = await _dbContext.Users
            .Where(u => u.LastUpdatePwd <= sixMonthsAgo 
                        && u.Status != changePwStatus)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            user.Status = changePwStatus;
        }

        if (users.Any())
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Update successfully {usersCount} users", users.Count);
        }

        return users;
    }
}
