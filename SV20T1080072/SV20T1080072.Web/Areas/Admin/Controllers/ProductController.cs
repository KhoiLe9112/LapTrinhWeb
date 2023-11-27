using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV20T1080072.BusinessLayers;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
	public class ProductController : Controller
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			
			var model = CommonDataService.ListOfProducts(out int rowCount, page, 5, searchValue);
            ViewBag.Page = page;
            ViewBag.RowCount = rowCount;
			ViewBag.TotalPage = (int)@Math.Ceiling((double)ViewBag.RowCount / 10);

            return View(model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Create()
		{
			ViewBag.Title = "Bổ sung mặt hàng";
			return View();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Edit(int id = 0)
		{
			ViewBag.Title = "Cập nhật mặt hàng";
			return View("Create");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Delete(int id = 0)
		{
			return View();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="method"></param>
		/// <param name="photoId"></param>
		/// <returns></returns>
		public IActionResult Photo(int id = 0, string method = "add", int photoId = 0)
		{
			switch (method)
			{
				case "add":
					ViewBag.Title = "Bổ sung ảnh";
					return View();
				case "edit":
					ViewBag.Title = "Thay đổi ảnh";
					return View();
				case "delete":
					//TODO: Delete photo
					return RedirectToAction("Edit", new { id = id });
				default:
					return RedirectToAction("Index");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="method"></param>
		/// <param name="attributeId"></param>
		/// <returns></returns>
		public IActionResult Attribute(int id = 0, string method = "add", int attributeId = 0)
		{
			switch (method)
			{
				case "add":
					ViewBag.Title = "Bổ sung thuộc tính";
					return View();
				case "edit":
					ViewBag.Title = "Thay đổi thuộc tính";
					return View();
				case "delete":
					//TODO: Delete Attribute
					return RedirectToAction("Edit", new { id = id });
				default:
					return RedirectToAction("Index");
			}
		}
	}
}
