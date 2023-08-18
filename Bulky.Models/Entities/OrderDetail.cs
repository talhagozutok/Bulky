using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.Entities;
public class OrderDetail
{
    public int Id { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }

    [Required]
    public int OrderHeaderId { get; set; }

    [ValidateNever]
    public OrderHeader OrderHeader { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ValidateNever]
    public Product Product { get; set; }
}
