using Microsoft.EntityFrameworkCore;
using TestSPA.Models;

public class AppDbContext2 : DbContext
{
    public AppDbContext2(DbContextOptions<AppDbContext2> options) : base(options) { }
}
