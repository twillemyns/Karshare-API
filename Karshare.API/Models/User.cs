using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [StringLength(50)]
    public required string Username { get; set; }
    
    [StringLength(50)]
    public required string PasswordHash { get; set; }
    
    [StringLength(50)]
    public required string FirstName { get; set; }
    
    [StringLength(50)]
    public required string LastName { get; set; }
    
    [EmailAddress]
    [StringLength(50)]
    public required string Email { get; set; }
    
    [Phone]
    [StringLength(10)]
    public required string PhoneNumber { get; set; }
    
    [StringLength(50)]
    public required string Address { get; set; }
    
    [StringLength(50)]
    public required string City { get; set; }
    
    [StringLength(50)]
    public required string Country { get; set; }
    
    public int Age { get; set; }
    
    public bool HasLicense { get; set; }
    
    public int YearsOfLicense { get; set; }
    
    public bool IsVerified { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public required Role Role { get; set; }

    public IEnumerable<Review> Reviews { get; set; } = [];
}