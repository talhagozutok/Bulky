using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.Entities;
public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    public int? CompanyId { get; set; }
    
    [ForeignKey(nameof(CompanyId))]
    [ValidateNever]
    public Company Company { get; set; }
}
