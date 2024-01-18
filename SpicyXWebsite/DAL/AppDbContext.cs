using Microsoft.EntityFrameworkCore;
using SpicyXWebsite.Models;

namespace SpicyXWebsite.DAL
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
		{

		}
		public DbSet<MenuItem> MenuItems { get; set; }
		public DbSet<Setting> Settings { get; set; }
    }
}
