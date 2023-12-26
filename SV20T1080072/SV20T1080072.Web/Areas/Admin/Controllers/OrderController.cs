using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080072.BusinessLayer;
using SV20T1080072.DomainModels;
using SV20T1080072.Web;
using SV20T1080072.Web.Models;
using System.Drawing.Printing;

namespace LiteCommerce.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Authorize(Roles = $"{WebUserRoles.Administrator}")]
	[Area("Admin")]
	public class OrderController : Controller
	{
		private const string CART = "CART";
		private const string CUSTOMER_LIST = "Customer_List";
		private const string EMPLOYEE_LIST = "Employee_List";
		private const string PRODUCT_SEARCH = "Product_Search";
		private const int PAGE_SIZE = 5;

		/// <summary>
		/// Hiển thị danh sách đơn hàng
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// Giao diện trang tạo đơn hàng
		/// </summary>
		/// <returns></returns>
		public IActionResult Create()
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

		/// <summary>
		/// Tìm kiếm và hiển thị mặt hàng cần đưa vào giỏ hàng
		/// </summary>
		/// <param name="page"></param>
		/// <param name="searchValue"></param>
		/// <returns></returns>
		public ActionResult SearchProduct(PaginationSearchInput input)
		{
			int rowCount = 0;
			var data = ProductDataService.ListProducts(input.Page, input.PageSize, input.SearchValue ?? "", 0, 0, 0, 0, out rowCount);
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
		/// Hiển thị giỏ hàng
		/// </summary>
		/// <returns></returns>
		public IActionResult ShowCart()
		{
			var model = GetCart();
			return View(model);
		}

		/// <summary>
		/// Lấy ds các mặt hàng trong giỏ
		/// </summary>
		/// <returns></returns>
		private List<CartItem> GetCart()
		{
			List<CartItem> cart = ApplicationContext.GetSessionData<List<CartItem>>(CART);
			if (cart == null)
			{
				cart = new List<CartItem>();
				ApplicationContext.SetSessionData(CART, cart);
			}
			return cart;
		}

		public IActionResult ShowCustomerList()
		{
			var model = GetCustomerList();
			return View(model);
		}

		public IActionResult ShowEmployeeList()
		{
			var model = GetEmployeeList();
			return View();
		}

		private List<Customer> GetCustomerList()
		{
			List<Customer> customer = ApplicationContext.GetSessionData<List<Customer>>(CUSTOMER_LIST);
			if (customer == null)
			{
				customer = new List<Customer>();
				ApplicationContext.SetSessionData(CUSTOMER_LIST, customer);
			}
			return customer;
		}

		private List<Employee> GetEmployeeList()
		{
			List<Employee> employee = ApplicationContext.GetSessionData<List<Employee>>(EMPLOYEE_LIST);
			if (employee == null)
			{
				employee = new List<Employee>();
				ApplicationContext.SetSessionData(EMPLOYEE_LIST, employee);
			}
			return employee;
		}

		/// <summary>
		/// Giao diện trang chi tiết đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Details(int id = 0)
		{
			return View();
		}
		/// <summary>
		/// Giao diện cập nhật thông tin chi tiết đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <param name="productId"></param>
		/// <returns></returns>        
		[HttpGet]
		public IActionResult EditDetail(int id = 0, int productId = 0)
		{
			return View();
		}
		/// <summary>
		/// Cập nhật chi tiết đơn hàng (trong giỏ hàng)
		/// </summary>
		/// <param name="id"></param>
		/// <param name="productId"></param>
		/// <param name="quantity"></param>
		/// <param name="salePrice"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult UpdateDetail(int id = 0, int productId = 0, int quantity = 0, decimal salePrice = 0)
		{
			return RedirectToAction("Details", new { id = id });
		}
		/// <summary>
		/// Xóa 1 mặt hàng khỏi giỏ hàng
		/// </summary>
		/// <param name="id"></param>
		/// <param name="productID"></param>
		/// <returns></returns>        
		public IActionResult DeleteDetail(int id = 0, int productID = 0)
		{
			return RedirectToAction("Details", new { id = id });
		}
		/// <summary>
		/// Xóa đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Delete(int id = 0)
		{
			//TODO: Code chức năng để xóa đơn hàng (nếu được phép xóa)

			return RedirectToAction("Index");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Accept(int id = 0)
		{
			//TODO: Duyệt chấp nhận đơn hàng

			return RedirectToAction("Details", new { id = id });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Shipping(int id = 0, int shipperID = 0)
		{
			if (Request.Method == "GET")
				return View();
			else
			{
				//TODO: Chuyển đơn hàng cho người giao hàng

				return RedirectToAction("Details", new { id = id });
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Finish(int id = 0)
		{
			//TODO: Ghi nhận hoàn tất đơn hàng

			return RedirectToAction($"Details", new { id = id });
		}
		/// <summary>
		/// Hủy bỏ đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Cancel(int id = 0)
		{
			//TODO: Hủy đơn hàng

			return RedirectToAction($"Details", new { id = id });
		}
		/// <summary>
		/// Từ chối đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Reject(int id = 0)
		{
			//TODO: Từ chối đơn hàng

			return RedirectToAction($"Details", new { id = id });
		}

		/// <summary>
		/// 
		/// </summary>        
		/// <returns></returns>
		[HttpPost]
		public IActionResult SearchProducts()
		{
			return RedirectToAction("Create");
		}
		/// <summary>
		/// Bổ sung thêm hàng vào giỏ hàng
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult AddToCart(CartItem data)
		{
			try
			{
				var cart = GetCart();
				int index = cart.FindIndex(m => m.ProductId == data.ProductId);
				if (index < 0)
				{
					cart.Add(data);
				}
				else
				{
					cart[index].Price = data.Price;
					cart[index].Quantity += data.Quantity;
				}
				ApplicationContext.SetSessionData(CART, cart);
				return Json("");
			}
			catch (Exception ex)
			{
				return Json(ex.Message);
			}
		}
		/// <summary>
		/// Xóa 1 mặt hàng khỏi giỏ hàng
		/// </summary>        
		/// <returns></returns>
		public ActionResult RemoveFromCart(string id)
		{
			var cart = GetCart();
			int index = cart.FindIndex(m => m.ProductId == id);
			if (index >= 0)
				cart.RemoveAt(index);
			ApplicationContext.SetSessionData(CART, cart);
			return Json("");
		}
		/// <summary>
		/// Xóa toàn bộ dữ liệu trong giỏ hàng
		/// </summary>
		/// <returns></returns>
		public ActionResult ClearCart()
		{
			var cart = GetCart();
			cart.Clear();
			ApplicationContext.SetSessionData(CART, cart);
			return Json("");
		}
		/// <summary>
		/// Khởi tạo đơn hàng và chuyển đến trang Details sau khi khởi tạo xong để tiếp tục quá trình xử lý đơn hàng
		/// </summary>        
		/// <returns></returns>
		[HttpPost]
		public ActionResult Init()
		{
			int orderId = 111;

			//TODO: Khởi tạo đơn hàng và nhận mã đơn hàng được khởi tạo

			return RedirectToAction("Details", new { id = orderId });
		}
	}
}
