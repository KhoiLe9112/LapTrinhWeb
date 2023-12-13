using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV20T1080072.BusinessLayer;
using SV20T1080072.BusinessLayers;
using SV20T1080072.Web.Models;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
	public class ProductController : Controller
	{
		private const string PRODUCT_SEARCH = "Customer_Search";
		private const int PAGE_SIZE = 10;
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{

			var input = ApplicationContext.GetSessionData<PaginationSearchInput>(PRODUCT_SEARCH);
			if (input == null)
			{
				input = new PaginationSearchInput()
				{
					Page = 1,
					PageSize = PAGE_SIZE,
					SearchValue = ""
				};
			}
			return View(input);
		}

		public IActionResult Search(PaginationSearchInput input)
		{
			int rowCount = 0;
			var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
			var model = new PaginationSearchProduct()
			{
				Page = input.Page,
				PageSize = input.PageSize,
				SearchValue = input.SearchValue ?? "",
				RowCount = rowCount,
				Data = data
			};

			//Lưu lại điều kiện tìm kiếm
			ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);

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
