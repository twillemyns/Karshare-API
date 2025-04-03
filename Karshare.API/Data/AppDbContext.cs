using Karshare.API.Models;
using Microsoft.EntityFrameworkCore;
using Route = Karshare.API.Models.Route;

namespace Karshare.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Route> Routes { get; set; }
    
    public DbSet<Car> Cars { get; set; }
    
    public DbSet<PassengerList> PassengerLists { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PassengerList>()
            .HasOne(p => p.Passenger)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}