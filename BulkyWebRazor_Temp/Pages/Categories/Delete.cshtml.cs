using BulkyWeb.Models;
using BulkyWebRazor_Temp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DeleteModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Category? Category { get; set; }

		public IActionResult OnGet([FromRoute(Name = "id")] int? id)
		{
			if (id is null)
			{
				return NotFound();
			}

			var category = _dbContext.Categories.Find(id);

			if (category is not null)
			{
				_dbContext.Categories.Remove(category);
				_dbContext.SaveChanges();
				TempData["delete"] = "Category deleted successfully";
				TempData["deleteText"] = $"Category name: {category.Name}";

				return RedirectToPage("Index");
			}

			return NotFound();
		}
	}
}
