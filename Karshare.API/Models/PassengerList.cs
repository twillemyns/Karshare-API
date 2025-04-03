namespace Karshare.API.Models;

public class PassengerList
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required User Passenger { get; set; }
    
    public required Route Route { get; set; }
}