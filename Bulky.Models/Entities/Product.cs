using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.Entities;

public class Product
{
	public int Id { get; set; }

	[Required(ErrorMessage = "{0} is required.")]
	[StringLength(100, ErrorMessage = "{0} can have a max of {1} characters.")]
	public string Title { get; set; } = string.Empty;

	[StringLength(550, ErrorMessage = "{0} can have a max of {1} characters.")]
	public string Description { get; set; } = string.Empty;

	[Required]
	public string ISBN { get; set; } = string.Empty;

	[Required]
	[StringLength(100, ErrorMessage = "{0} can have a max of {1} characters.")]
	public string Author { get; set; } = string.Empty;


	[Required]
	[Display(Name = "Price for 1-50")]
	[Range(minimum: 1, maximum: 100_000, ErrorMessage = "{0} must be between {1} and {2}.")]
	public double ListPrice { get; set; }

	[Required]
	[Range(minimum: 1, maximum: 100_000, ErrorMessage = "{0} must be between {1} and {2}.")]
	public double Price { get; set; }

	[Required]
	[Display(Name = "Price for 50+")]
	[Range(minimum: 1, maximum: 100_000, ErrorMessage = "{0} must be between {1} and {2}.")]
	public double PriceFifty { get; set; }

	[Required]
	[Display(Name = "Price for 100+")]
	[Range(minimum: 1, maximum: 100_000, ErrorMessage = "{0} must be between {1} and {2}.")]
	public double PriceHundredOrMore { get; set; }

	[Display(Name = "Category")]
	public int CategoryId { get; set; }

	[ForeignKey(nameof(CategoryId))]
	[ValidateNever]
	public Category? Category { get; set; }

	[ValidateNever]
	[Display(Name = "Images")]
	public List<ProductImage> ProductImages { get; set; }
}
