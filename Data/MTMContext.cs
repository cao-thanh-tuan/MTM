using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MTM.Models;

namespace MTM.Data
{
    public class MTMContext : DbContext
    {
        public MTMContext(DbContextOptions<MTMContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Disciple> Disciples { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<DataPoint> DataPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Class>().ToTable("Class");
            modelBuilder.Entity<Disciple>().ToTable("Disciple");
            modelBuilder.Entity<Registration>().ToTable("Registration");
        }

    }
}
