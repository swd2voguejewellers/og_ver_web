using Microsoft.EntityFrameworkCore;
using TestSPA.Models;

public class AppDbContext1 : DbContext
{
    public DbSet<RatesDetails> ratesDetails { get; set; }
    public AppDbContext1(DbContextOptions<AppDbContext1> options) : base(options) { }
}
