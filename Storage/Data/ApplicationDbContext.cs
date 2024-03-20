using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Storage.Data.StorageDb;

namespace Storage.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
		public DbSet<User> Users { get; set; }
		public DbSet<StorageItem> StorageItems { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<StoragePlace> StoragePlaces { get; set; }
		public DbSet<StoragePlaceCell> StoragePlacesCells { get; set; }
	}
	
}
