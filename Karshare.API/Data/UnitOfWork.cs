using Karshare.API.Models;

namespace Karshare.API.Data;

public class UnitOfWork(AppDbContext context) : IDisposable
{
    public GenericRepository<User> UserRepository = new(context);
    public GenericRepository<Role> RoleRepository = new(context);
    public GenericRepository<Car> CarRepository = new(context);
    public GenericRepository<PassengerList> PassengerListRepository = new(context);
    public GenericRepository<Review> ReviewRepository = new(context);
    public GenericRepository<Trip> TripRepository = new(context);
    
    public async Task<int> Save() => await context.SaveChangesAsync();

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}