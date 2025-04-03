using System.ComponentModel.DataAnnotations;

namespace Karshare.API.Models;

public class Role
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [StringLength(25)]
    public required string Name { get; set; }
}