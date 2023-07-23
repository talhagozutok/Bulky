using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Entities;

public class Category
{
	public int Id { get; set; }

	[Display(Name = "Category name")]
	[Required(ErrorMessage = "{0} is required.")]
	[StringLength(50, ErrorMessage = "{0} can have a max of {1} characters.")]
	public string Name { get; set; } = string.Empty;

	[Display(Name = "Display order")]
	[Range(minimum: 1, maximum: double.PositiveInfinity, ErrorMessage = "{0} must be between {1} and {2}.")]
	public int DisplayOrder { get; set; }
}
