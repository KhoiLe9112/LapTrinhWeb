using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080072.BusinessLayers;
using SV20T1080072.DomainModels;
using SV20T1080072.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
	public class ShipperController : Controller
	{
		private const int PAGE_SIZE = 10;
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			int rowCount = 0;
			var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchShipper()
			{
				Page = page,
				PageSize = PAGE_SIZE,
				SearchValue = searchValue ?? "",
				RowCount = rowCount,
				Data = data
			};

			string? errorMessage = Convert.ToString(TempData["ErrorMessage"]);
			ViewBag.ErrorMessage = errorMessage;

			return View(model);
		}
		public IActionResult Create()
		{
			var model = new Shipper()
			{
				ShipperID = 0
			};
			ViewBag.Title = "Bổ sung người giao hàng";
			return View(model);
		}

		public IActionResult Edit(int id = 0)
		{
			var model = CommonDataService.GetShipper(id);
			if (model == null)
			{
				return RedirectToAction("Index");
			}
			ViewBag.Title = "Cập nhật người giao hàng";
			return View("Create", model);
		}

		public IActionResult Delete(int id = 0)
		{
			if (Request.Method == "POST")
			{
				bool success = CommonDataService.DeleteShipper(id);
				if (!success)
					TempData["ErrorMessage"] = "Không thể xóa người giao hàng này";
				return RedirectToAction("Index");
			}
			var model = CommonDataService.GetShipper(id);
			if (model == null)
				return RedirectToAction("Index");
			return View(model);
		}

		public IActionResult Save(Shipper data)
		{
			ViewBag.Title = data.ShipperID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";

			if (string.IsNullOrWhiteSpace(data.ShipperName))
				ModelState.AddModelError(nameof(data.ShipperName), "Tên đơn vị giao hàng không được rỗng");
			else if (CheckString.ContainsSpecial(data.ShipperName) == true)
				ModelState.AddModelError(nameof(data.ShipperName), "Tên đơn vị giao hàng không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Phone))
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được rỗng");
			else if (CheckString.IsPhone(data.Phone) == true)
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");

			if (!ModelState.IsValid)
			{
				return View("Create", data);
			}

			if (data.ShipperID == 0)
			{
				int shipperId = CommonDataService.AddShipper(data);
				if (shipperId > 0)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateShipper(data);
				if (success)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
				return View("Create", data);
			}
		}
	}
}
