using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class Car
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [StringLength(25)]
    public required string Brand { get; set; }
    
    [StringLength(25)]
    public required string Model { get; set; }
    
    [StringLength(25)]
    public required string Color { get; set; }
    
    public int NbSeats { get; set; }
    
    [StringLength(7)]
    public required string Plate { get; set; }
    
    public bool IsLuggageAvailable { get; set; }
    
    public required Trip User { get; set; }
}