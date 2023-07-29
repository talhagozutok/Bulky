using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Bulky.Models.Entities;
public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? StreetAdress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
}
