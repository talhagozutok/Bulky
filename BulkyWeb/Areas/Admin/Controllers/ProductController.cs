using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var productList = _unitOfWork.Products.GetAll(includeProperties: nameof(Category)).ToList();
        return View(productList);
    }

    // [UP]date and in[SERT] functionality.
    public IActionResult Upsert([FromRoute(Name = "id")] int? id)
    {
        ProductViewModel viewModel = new()
        {
            Product = new Product(),
            CategoryList = _unitOfWork.Categories
                .GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
        };

        if (id is null || id == 0)
        {
            // Create
            return View(viewModel);
        }

        // Update
        Product? product = _unitOfWork.Products.Get(p => p.Id.Equals(id),
            includeProperties: "ProductImages");
        if (product is not null)
        {
            viewModel.Product = product;
            return View(viewModel);
        }

        return NotFound();
    }

    [HttpPost]
    public IActionResult Upsert(ProductViewModel viewModel, List<IFormFile> files)
    {
        if (ModelState.IsValid)
        {
            // We are creating/updating product first
            // then we use its id to upload the images.
            if (viewModel.Product.Id == 0)
            {
                _unitOfWork.Products.Add(viewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.Products.Update(viewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
            }

            if (files is not null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                foreach (IFormFile file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\products\product-" + viewModel.Product.Id;
                    string productImagesPath = Path.Combine(wwwRootPath, productPath);
                    if (!Directory.Exists(productImagesPath))
                    {
                        Directory.CreateDirectory(productImagesPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(productImagesPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    ProductImage productImage = new()
                    {
                        ImageUrl = @"\" + productPath + @"\" + fileName,
                        ProductId = viewModel.Product.Id
                    };

                    if (viewModel.Product.ProductImages is null)
                    {
                        viewModel.Product.ProductImages = new List<ProductImage>();
                    }

                    viewModel.Product.ProductImages.Add(productImage);
                }

                _unitOfWork.Products.Update(viewModel.Product);
                _unitOfWork.Save();
            }


            return RedirectToAction("Index");
        }
        else
        {
            viewModel.CategoryList = _unitOfWork.Categories
                .GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });

            return View(viewModel);
        }
    }

    public IActionResult DeleteImage(int imageId)
    {
        var image = _unitOfWork.ProductImages.Get(i => i.Id == imageId);

        if (image is not null)
        {
            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // If second parameter starts with backslash(\)
                // Path.Combine(string path1, string path2)
                // will return second parameter because it is an absolute path.
                // In this case image.ImageUrl.TrimStart('\\') used
                // because second parameter contains
                // an absolute path which is \product\..
                // TrimStart method trims the string into product\...
                string oldImagePath = Path.Combine(wwwRootPath, 
                    image.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Delete imageDirectory if no files are remaining.
                string imageDirectory = Path.Combine(wwwRootPath,
                    Path.GetDirectoryName(image.ImageUrl).Trim('\\'));
                if (!Directory.EnumerateFiles(imageDirectory).Any())
                {
                    Directory.Delete(imageDirectory);
                }

                _unitOfWork.ProductImages.Remove(image);
                _unitOfWork.Save();
                TempData["delete"] = "Image deleted successfully";

                return RedirectToAction(nameof(Upsert), new { id = image.ProductId });
            }
        }

        return NotFound();
    }

    public IActionResult Edit([FromRoute(Name = "id")] int? id)
    {
        var product = _unitOfWork.Products.Get(p => p.Id.Equals(id));
        return product is not null ? View(product) : NotFound();
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = _unitOfWork.Products.Get(p => p.Id.Equals(id));

        if (product is not null)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageDirectory = Path.Combine(wwwRootPath,
                    @"images\products\product-" + id);

            if (Directory.Exists(imageDirectory))
            {
                Directory.Delete(imageDirectory, true);
            }

            _unitOfWork.Products.Remove(product);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        return NotFound();
    }

    #region API

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Products.GetAll(includeProperties: nameof(Category)).ToList();
        return Json(new { data = productList });
    }

    #endregion
}
