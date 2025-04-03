using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class Review
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public int NbStars { get; set; }
    
    [StringLength(250)]
    public required string Comment { get; set; }
    
    public required User Reviewer { get; set; }
    
    public DateTime CreatedAt { get; set; }
}