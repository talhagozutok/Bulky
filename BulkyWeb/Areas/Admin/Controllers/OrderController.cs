using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;
public class OrderController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public OrderController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpDelete]
	public IActionResult Delete([FromRoute(Name = "id")] int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var order = _unitOfWork.OrderHeaderRepository.Get(p => p.Id.Equals(id));

		if (order is not null)
		{
			_unitOfWork.OrderHeaderRepository.Remove(order);
			_unitOfWork.Save();

			return RedirectToAction("Index");
		}

		return NotFound();
	}

	#region API

	[HttpGet]
	public IActionResult GetAll()
	{
		List<OrderHeader> orderHeaderList = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: nameof(ApplicationUser)).ToList();
		return Json(new { data = orderHeaderList });
	}

	#endregion
}
