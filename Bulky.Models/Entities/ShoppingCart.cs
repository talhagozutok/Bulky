using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.Entities;
public class ShoppingCart
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    [ValidateNever]
    public Product? Product { get; set; }

    [Range(1, 1000, ErrorMessage = "Please enter a value between {1} and {2}")]
    public int Count { get; set; }
    public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get;}

    [NotMapped]
    public double Price { get; set; }
}
