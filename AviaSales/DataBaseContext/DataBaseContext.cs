using AviaSales.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFood> TicketFoods { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-KIV92L3;Database=AviaSales;Trusted_Connection=True;Encrypt=False;");
            // optionsBuilder.UseSqlServer("Server=DESKTOP-I75L3P7;Database=AviaSales;Trusted_Connection=True;Encrypt=False;");
            // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AviaSales;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Airport>().HasMany(x => x.Departures).WithOne(x => x.DepartureAirport)
                        .HasForeignKey(x => x.DepartureAirportId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Airport>().HasMany(x => x.Arrivals).WithOne(x => x.ArrivalAirport)
                        .HasForeignKey(x => x.ArrivalAirportId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
