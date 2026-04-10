using Microsoft.EntityFrameworkCore;
using TestSPA.Interfaces;

namespace TestSPA.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext1 _context;

        public AuthRepository(AppDbContext1 context)
        {
            _context = context;
        }

        public async Task<AppUser?> ValidateUserAsync(string username, string password)
        {
            username = username.Trim();
            password = password.Trim();

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.UserName != null &&
                    x.Password != null &&
                    !x.Off &&
                    x.UserName == username &&
                    x.Password == password);
        }
    }
}
