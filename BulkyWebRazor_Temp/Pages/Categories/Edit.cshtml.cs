using BulkyWeb.Models;
using BulkyWebRazor_Temp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
        public Category? Category { get; set; }
        
        public void OnGet(int id)
        {
            Category = _dbContext.Categories.Find(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) {
                return Page();
            }
            

            if(Category is not null)
            {
				_dbContext.Categories.Update(Category);
				_dbContext.SaveChanges();
				TempData["success"] = "Category updated successfully";

            }

            return RedirectToPage("Index");
        }
    }
}
