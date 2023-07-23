using BulkyWeb.Models;
using BulkyWebRazor_Temp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Category? Category { get; set; }

		public void OnGet()
		{

		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Category is not null)
			{
				_dbContext.Categories.Add(Category);
				_dbContext.SaveChanges();
				TempData["success"] = "Category created successfully";
			}

			return RedirectToPage("Index");
		}
	}
}
