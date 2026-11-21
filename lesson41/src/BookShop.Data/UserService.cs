using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class UserService : IUserService
{
    private readonly BookShopDbContext _context;

    public UserService(BookShopDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> FindUserAsync(string userName, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}
