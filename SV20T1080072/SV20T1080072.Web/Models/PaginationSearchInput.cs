﻿using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	/// <summary>
	///Lưu thông tin tìm kiếm vào session
	/// </summary>
	public class PaginationSearchInput
	{
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public string SearchValue { get; set; } = "";
		public int CategoryID { get; set; } = 0;
		public int SupplierID { get; set; } = 0;
        public int Status { get; set; }
		public int customerID { get; set; }
		public int shipperID { get; set; }
		public int employeeID { get; set; }
	}
}
