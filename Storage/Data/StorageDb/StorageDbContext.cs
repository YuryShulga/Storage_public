using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Data.StorageDb
{
    internal class StorageDbContext : DbContext
    {
        //private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=StorageDb;Trusted_Connection=True;";
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}

        public StorageDbContext(DbContextOptions<StorageDbContext> options)
           : base(options)
        {
            //проверяю существует ли база данных
			if (Database.CanConnect() == false)
			{
                Database.Migrate();
			}
		}

        public DbSet<User> Users { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<StoragePlace> StoragePlaces { get; set; }
        public DbSet<StoragePlaceCell> StoragePlacesCells { get; set; }

        
    }

}
