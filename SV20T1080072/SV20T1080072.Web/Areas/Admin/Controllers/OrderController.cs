using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080072.BusinessLayer;
using SV20T1080072.BusinessLayers;
using SV20T1080072.DomainModels;
using SV20T1080072.Web;
using SV20T1080072.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080072.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Authorize(Roles = $"{WebUserRoles.Administrator}")]
	[Area("Admin")]
	public class OrderController : Controller
	{
		private const string CART = "CART";
		//private const string CUSTOMER_LIST = "Customer_List";
		//private const string EMPLOYEE_LIST = "Employee_List";
		private const string PRODUCT_SEARCH = "Product_Search";
        private const string ORDER_SEARCH = "Order_Search";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const int PAGE_SIZE = 5;

		/// <summary>
		/// Hiển thị danh sách đơn hàng
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                };
            }
            return View(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult SearchOrder(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(input.Page, input.PageSize, input.Status, input.SearchValue, out rowCount);
            var model = new PaginationSearchOrder()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH, input);//lưu lại điều kiện tìm kiếm
            return View(model);
        }

        /// <summary>
        /// Giao diện trang chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order order = OrderDataService.GetOrder(id);
            List<OrderDetail> orderDetails = OrderDataService.ListOrderDetails(id);

            PaginationSearchOrderDetail result = new PaginationSearchOrderDetail()
            {
                Order = order,
                OrderDetails = orderDetails
            };
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(result);
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
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productId < 0)
            {
                return RedirectToAction("Details", new { id = productId });
            }
            OrderDetail orderDetail = OrderDataService.GetOrderDetail(id, productId);
            if (orderDetail == null)
            {
                return RedirectToAction("Index");
            }
            return View(orderDetail);
        }

        /// <summary>
		/// Giao diện tạo mới thông tin chi tiết đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult CreateDetail(int id = 0)
        {
            var model = new OrderDetail()
            {
                OrderID = id,
                ProductID = 0
            };
            return View(model);
        }

        /// <summary>
		/// Lưu thông tin chi tiết đơn hàng sau khi cập nhật
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public IActionResult SaveEditDetail(OrderDetail data)
        {
            // mã đặt hàng
            if (data.ProductID <= 0)
            {
                TempData[ERROR_MESSAGE] = "mã đặt hàng không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            // Số lượng
            if (data.Quantity < 1)
            {
                TempData[ERROR_MESSAGE] = "Số lượng không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            // Đơn giá
            if (data.SalePrice < 1)
            {
                TempData[ERROR_MESSAGE] = "Đơn giá không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }

            // Cập nhật chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return RedirectToAction("Details", new { id = data.OrderID });
        }

        /// <summary>
		/// Xóa 1 mặt hàng ra khỏi danh sách chi tiết đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <param name="productID"></param>
		/// <returns></returns>
		public IActionResult DeleteDetail(int id = 0, int productID = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productID < 0)
            {
                return RedirectToAction("Details", new { id = id });
            }

            bool isDeleted = OrderDataService.DeleteOrderDetail(id, productID);
            if (!isDeleted)
            {
                TempData[ERROR_MESSAGE] = "Không thể xoá mặt hàng này";
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
		/// Lưu thông tin chi tiết đơn hàng sau khi xóa
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult SaveDeleteDetail(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            if (data.Status == OrderStatus.INIT
                || data.Status == OrderStatus.CANCEL
                || data.Status == OrderStatus.REJECTED)
            {
                OrderDataService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
		/// Chấp nhận đơn hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Accept(int id = 0)
        {
            //TODO: Duyệt chấp nhận đơn hàng

            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            bool isAccepted = OrderDataService.AcceptOrder(id);
            if (!isAccepted)
            {
                TempData[ERROR_MESSAGE] = $"Chấp nhận đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction($"Details/{data.OrderID}");
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
		/// Trạng thái giao hàng
		/// </summary>
		/// <param name="id"></param>
		/// <param name="shipperID"></param>
		/// <param name="countProducts"></param>
		/// <returns></returns>
		public IActionResult Shipping(int id = 0, int shipperID = 0, int countProducts = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (Request.Method == "GET")
            {
                ViewBag.OrderID = id;
                ViewBag.CountProducts = countProducts;
                return View();
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            if (shipperID <= 0)
            {
                TempData[ERROR_MESSAGE] = "Bạn phải chọn đơn vị vận chuyển";
                return RedirectToAction("Details", new { id = id });
            }
            if (countProducts <= 0)
            {
                TempData[ERROR_MESSAGE] = "Không có mặt hàng nào để chuyển giao";
                return RedirectToAction("Details", new { id = id });
            }
            bool isShipped = OrderDataService.ShipOrder(id, shipperID);
            if (!isShipped)
            {
                TempData[ERROR_MESSAGE] = $"Xác nhận chuyển đơn hàng cho người giao hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            //else
            //{
            //    var shipper = CommonDataService.GetShipper(shipperID);
            //    if (shipper == null)
            //    {
            //        TempData["ErrorMessage"] = "Người giao hàng không tồn tại";
            //        return RedirectToAction("Details", new { id = id });
            //    }
            //    data.Status = 3;
            //    data.ShipperID = shipperID;
            //    data.ShippedTime = DateTime.Now;
            //    bool success = OrderDataService.ShipOrder(data)
            //}
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Trạng thái hoàn tất đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            bool isFinished = OrderDataService.FinishOrder(id);
            if (!isFinished)
            {
                TempData[ERROR_MESSAGE] = $"Xác nhận hoàn tất đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            bool isCanceled = OrderDataService.CancelOrder(id);
            if (!isCanceled)
            {
                TempData[ERROR_MESSAGE] = $"Hủy bỏ đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            //Code chức năng từ chối đơn hàng (nếu được phép)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            bool isRejected = OrderDataService.RejectOrder(id);
            if (!isRejected)
            {
                TempData[ERROR_MESSAGE] = $"Từ chối đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
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
			ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
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
            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);

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

        /// <summary>
        /// Cập nhật chi tiết đơn hàng (trong giỏ hàng)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateDetail(OrderDetail data)
        {
            // mã đặt hàng
            if (data.ProductID <= 0)
            {
                TempData[ERROR_MESSAGE] = "mã đặt hàng không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            // Số lượng
            if (data.Quantity < 1)
            {
                TempData[ERROR_MESSAGE] = "Số lượng không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }

            // Đơn giá
            if (data.SalePrice < 1)
            {
                TempData[ERROR_MESSAGE] = "Đơn giá không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }

            // Cập nhật chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return RedirectToAction("Details", new { id = data.OrderID });
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
            List<CartItem> cart = GetCart();
            int index = cart.FindIndex(m => m.ProductId == id);
            if (index >= 0)
                cart.RemoveAt(index);
            ApplicationContext.SetSessionData(CART, cart);
            return RedirectToAction("Create");
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
            return RedirectToAction("Create");
        }

        /// <summary>
        /// Khởi tạo đơn hàng và chuyển đến trang Details sau khi khởi tạo xong để tiếp tục quá trình xử lý đơn hàng
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            List<CartItem> shoppingCart = GetCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            if (customerID == 0 || employeeID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng và nhân viên phụ trách";
                return RedirectToAction("Create");
            }
            // Chuyển đổi từ List<CartItem> sang List<OrderDetail>
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (CartItem cartItem in shoppingCart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    // Thiết lập các thuộc tính của OrderDetail từ CartItem
                    ProductID = Convert.ToInt32(cartItem.ProductId),
                    ProductName = cartItem.ProductName,
                    Unit = cartItem.Unit,
                    Quantity = cartItem.Quantity,
                    SalePrice = cartItem.Price


                    // Các thuộc tính khác của OrderDetail mà bạn có thể cần thiết lập
                };

                orderDetails.Add(orderDetail);
            }

            int orderID = OrderDataService.InitOrder(customerID, employeeID, DateTime.Now, orderDetails);

            HttpContext.Session.Remove(CART);
            return RedirectToAction("Details", new { id = orderID });
        }
    }
}
