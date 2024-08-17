using BackgroundJob.Models;

namespace BackgroundJob.Services;
public interface IUserService
{
    Task<IEnumerable<User>> UpdatePasswordAsync(CancellationToken cancellationToken);
}
