namespace Karshare.API.Models;

public class PassengerList
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required Trip Passenger { get; set; }
    
    public required Trip Route { get; set; }
}