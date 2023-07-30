using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Entities;
public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
	[Display(Name = "Company Name")]
	public string Name { get; set; } = string.Empty;

    public string? City { get; set; }
    public string? State { get; set; }

    [Display(Name="Street Adress")]
    public string? StreetAddress { get; set; }

	[Display(Name = "Postal Code")]
	public string? PostalCode { get; set; }

	[Display(Name = "Phone Number")]
	public string? PhoneNumber { get; set; }
}
