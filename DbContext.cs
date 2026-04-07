using Microsoft.EntityFrameworkCore;
using TestSPA.Models;

public class AppDbContext : DbContext
{
    public DbSet<OGVerification> OGVerifications { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
