using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080072.BusinessLayers;
using SV20T1080072.DomainModels;
using SV20T1080072.Web.Models;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
	public class SupplierController : Controller
	{
		private const int PAGE_SIZE = 10;
		public IActionResult Index(int page = 1, string searchValue = "")
		{

			int rowCount = 0;
			var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchSupplier()
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
			var model = new Supplier()
			{
				SupplierID = 0
			};
			ViewBag.Title = "Bổ sung nhà cung cấp";
			return View(model);
		}

		public IActionResult Edit(int id = 0)
		{
			var model = CommonDataService.GetSupplier(id);
			if (model == null)
			{
				return RedirectToAction("Index");
			}
			ViewBag.Title = "Cập nhật nhà cung cấp";
			return View("Create", model);
		}

		public IActionResult Delete(int id = 0)
		{
			if (Request.Method == "POST")
			{
				bool success = CommonDataService.DeleteSupplier(id);
				if (!success)
					TempData["ErrorMessage"] = "Không thể xóa nhà cung cấp này";
				return RedirectToAction("Index");
			}
			var model = CommonDataService.GetSupplier(id);
			if (model == null)
				return RedirectToAction("Index");
			return View(model);
		}

		public IActionResult Save(Supplier data)
		{
			ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";

			if (string.IsNullOrWhiteSpace(data.SupplierName))
				ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được rỗng");
			else if (CheckString.ContainsSpecial(data.SupplierName) == true)
				ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.ContactName))
				ModelState.AddModelError(nameof(data.ContactName), "Tên liên lạc không được rỗng");
			else if (CheckString.ContainsSpecial(data.ContactName) == true)
				ModelState.AddModelError(nameof(data.ContactName), "Tên liên lạc không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Address))
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được rỗng");
			else if (CheckString.ContainsSpecial(data.Address) == true)
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Province))
				ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành");
			//else if (provinces != data.Province)
			//	ModelState.AddModelError(nameof(data.Province), "Tỉnh/thành không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Phone))
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được rỗng");
			else if (CheckString.IsPhone(data.Phone) == false)
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Email))
				ModelState.AddModelError(nameof(data.Email), "Địa chỉ email không được rỗng");
			else if (CheckString.IsEmail(data.Email) == false)
				ModelState.AddModelError(nameof(data.Email), "Địa chỉ email không hợp lệ");

			if (!ModelState.IsValid)
			{
				return View("Create", data);
			}

			if (data.SupplierID == 0)
			{
				int suppplierId = CommonDataService.AddSupplier(data);
				if (suppplierId > 0)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateSupplier(data);
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
