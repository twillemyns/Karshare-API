using Karshare.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Karshare.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<Trip> Trips { get; set; }
    
    public virtual DbSet<Car> Cars { get; set; }
    
    public virtual DbSet<PassengerList> PassengerLists { get; set; }
    
    public virtual DbSet<Role> Roles { get; set; }
    
    public virtual DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PassengerList>()
            .HasOne(p => p.Passenger)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}