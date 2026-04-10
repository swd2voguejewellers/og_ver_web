namespace TestSPA.Interfaces
{
    public interface IAuthRepository
    {
        Task<AppUser?> ValidateUserAsync(string username, string password);
    }
}
