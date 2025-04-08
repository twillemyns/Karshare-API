using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class Trip
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required User CreatedUser { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [StringLength(50)]
    public required string StartCity { get; set; }
    
    [StringLength(50)]
    public required string EndCity { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public double Distance { get; set; }
    
    public double Price { get; set; }
    
    [StringLength(250)]
    public required string Description { get; set; }
    
    public bool IsAnimalAccepted { get; set; }
    
    public bool IsSmokerAccepted { get; set; }
    
    [StringLength(250)]
    public required string RadioDescription { get; set; }
    
    public bool IsTalkingAccepted { get; set; }
    
    public bool AreKidsAccepted { get; set; }
}