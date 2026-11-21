namespace BookShop.Data;

public interface IUserService
{
    Task<AppUser?> FindUserAsync(string userName, string password);
}
