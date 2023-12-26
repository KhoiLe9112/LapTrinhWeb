using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV20T1080072.BusinessLayer;
using SV20T1080072.BusinessLayers;
using SV20T1080072.DomainModels;
using SV20T1080072.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
	public class ProductController : Controller
	{
		private const string PRODUCT_SEARCH = "Product_Search";
		private const int PAGE_SIZE = 5;
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
					SearchValue = "",
					CategoryID = 0,
					SupplierID = 0,
				};
			}
			return View(input);
		}

		public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID, 0, 0, out rowCount);
            var model = new PaginationSearchProduct()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
				categoryID = input.CategoryID,
				supplierID = input.SupplierID
			};

            //Lưu lại điều kiện tìm kiếm
			ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);

			string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
			ViewBag.ErrorMessage = errorMessage;
			string deletedMessage = Convert.ToString(TempData["DeletedMessage"]);
			ViewBag.DeletedMessage = deletedMessage;
			string savedMessage = Convert.ToString(TempData["SavedMessage"]);
			ViewBag.SavedMessage = savedMessage;

			return View(model);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Create()
		{
            ViewBag.Title = "Bổ sung mặt hàng";
            var data = new Product()
            {
                ProductId = 0
            };
            return View(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Edit(int id = 0)
		{
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật mặt hàng";
            return View("Create", model);
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

        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.ProductId == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";

            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được rỗng!");
            if (data.CategoryId == 0)
                ModelState.AddModelError(nameof(data.CategoryId), "Vui lòng chọn loại hàng!");
            if (data.SupplierId == 0)
                ModelState.AddModelError(nameof(data.SupplierId), "Vui lòng chọn nhà cung cấp!");
            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được rỗng!");
            if (data.Price == 0)
                ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá sản phẩm!");

            //Xử lý với ảnh
            //Upload ảnh lên (nếu có), sau khi upload xong thì mới lấy tên file ảnh vừa upload
            //để gán cho trường Photo của Employee
            if (uploadPhoto != null)
            {
                string fileName = $"{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\Products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            //Kiểm tra đầu vào của model

            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.ProductId == 0)
            {
                int productId = ProductDataService.AddProduct(data);
                if (productId > 0)
                {
                    TempData["SavedMessage"] = "Bổ sung mặt hàng thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không bổ sung được dữ liệu!";
                    return View("Create", data);
                }
            }
            else
            {
                bool editSuccess = ProductDataService.UpdateProduct(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Cập nhật thông tin mặt hàng thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không cập nhật được thông tin mặt hàng!";
                    return View("Edit", data);
                }
            }
        }

    }
}
