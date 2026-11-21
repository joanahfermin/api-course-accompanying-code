using BookShop.Data;

namespace BookShop.Api.Tests;

public class FakeUserService : IUserService
{
    public Task<AppUser?> FindUserAsync(string userName, string password)
    {
        if (userName == "alice" && password == "secret")
        {
            return Task.FromResult<AppUser?>(new AppUser { UserName = "alice", Role = "Manager" });
        }

        return Task.FromResult<AppUser?>(null);
    }
}
