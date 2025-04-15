using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class Trip
{

    public Guid Id { get; init; } = Guid.NewGuid();
    [Required]
    public Trip CreatedUser { get; set; }
    
    public DateTime CreatedAt { get; set; }

    [Required]
    [StringLength(50)]
    public string StartCity { get; set; }

    [Required]
    [StringLength(50)]
    public string EndCity { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public double Distance { get; set; }
    
    public double Price { get; set; }

    [Required]
    [StringLength(250)]
    public string Description { get; set; }
    
    public bool IsAnimalAccepted { get; set; }
    
    public bool IsSmokerAccepted { get; set; }

    [StringLength(250)]
    public string RadioDescription { get; set; }
    
    public bool IsTalkingAccepted { get; set; }
    
    public bool AreKidsAccepted { get; set; }
}