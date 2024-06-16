using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Bookings)
        //        .WithOne(b => b.Customer)
        //        .HasForeignKey(b => b.CustomerId);

        //    modelBuilder.Entity<Room>()
        //        .HasMany(r => r.Bookings)
        //        .WithOne(b => b.Room)
        //        .HasForeignKey(b => b.RoomId);
        //}
    }
}
