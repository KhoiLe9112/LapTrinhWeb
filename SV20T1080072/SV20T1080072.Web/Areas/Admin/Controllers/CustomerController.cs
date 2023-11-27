using Microsoft.AspNetCore.Mvc;
using SV20T1080072.BusinessLayers;
using PagedList;
using SV20T1080072.Web.Models;
using SV20T1080072.DomainModels;
using SV20T1080072.DataLayers.SQLServer;
using Microsoft.AspNetCore.Authorization;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
	[Authorize(Roles = $"{WebUserRoles.Administrator}")]
	[Area("Admin")]
	public class CustomerController : Controller
	{
		private const int PAGE_SIZE = 10;
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			//var model = CommonDataService.ListOfCustomers(out int rowCount, page, 10, searchValue);
			//ViewBag.Page = page;
			//ViewBag.RowCount = rowCount;
			//ViewBag.TotalPage = (int)@Math.Ceiling((double)ViewBag.RowCount / 10);
			//return View(model);

			int rowCount = 0;
			var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchCustomer()
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
			var model = new Customer()
			{
				CustomerID = 0
			};
			ViewBag.Title = "Bổ sung khách hàng";
			return View(model);
		}

		public IActionResult Edit(int id = 0)
		{
			var model = CommonDataService.GetCustomer(id);
			if (model == null)
			{
				return RedirectToAction("Index");
			}
			ViewBag.Title = "Cập nhật khách hàng";
			return View("Create", model);
		}

		public IActionResult Delete(int id = 0)
		{
			if (Request.Method == "POST")
			{
				bool success = CommonDataService.DeleteCustomer(id);
				if (!success)
					TempData["ErrorMessage"] = "Không thể xóa khách hàng này";
				return RedirectToAction("Index");
			}
			var model = CommonDataService.GetCustomer(id);
			if (model == null)
				return RedirectToAction("Index");
			return View(model);
		}

		public IActionResult Save(Customer data, string provinces)
		{
			ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";

			if (string.IsNullOrWhiteSpace(data.CustomerName))
				ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được rỗng");
			else if (CheckString.ContainsNumber(data.CustomerName) == true || CheckString.ContainsSpecial(data.CustomerName) == true)
				ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không hợp lệ");
			
			if (string.IsNullOrWhiteSpace(data.ContactName))
				ModelState.AddModelError(nameof(data.ContactName), "Tên liên lạc không được rỗng");
			else if (CheckString.ContainsSpecial(data.ContactName) == true)
				ModelState.AddModelError(nameof(data.ContactName), "Tên liên lạc không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Province))
				ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành");
			//else if (ProvinceDAL.Count(data.Province) = 0)
			//	ModelState.AddModelError(nameof(data.Province), "Tỉnh/thành không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Address))
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được rỗng");
			else if (CheckString.ContainsSpecial(data.Address) == true)
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Phone))
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được rỗng");
			else if (CheckString.IsPhone(data.Phone) == false)
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Email))
				ModelState.AddModelError(nameof(data.Email), "Email không được rỗng");
			else if (CheckString.IsEmail(data.Email) == false)
				ModelState.AddModelError(nameof(data.Email), "Email không hợp lệ");

			if (!ModelState.IsValid)
			{
				return View("Create", data);
			}

			if (data.CustomerID == 0) {
				int customerId = CommonDataService.AddCustomer(data);
				if (customerId > 0)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateCustomer(data);
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

