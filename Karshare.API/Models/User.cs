using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class User
{
    [Required]
    public Guid Id { get; init; } = Guid.NewGuid();
    [Required]
    [StringLength(50)]
    public string? Username { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; set; }

    [Required]
    [Phone]
    [StringLength(10)]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string? Address { get; set; }

    [Required]
    [StringLength(50)]
    public string? City { get; set; }

    [Required]
    [StringLength(50)]
    public string? Country { get; set; }
    
    public int Age { get; set; }
    
    public bool HasLicense { get; set; }
    
    public int YearsOfLicense { get; set; }
    
    public bool IsVerified { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public IEnumerable<Review> Reviews { get; set; } = [];
}