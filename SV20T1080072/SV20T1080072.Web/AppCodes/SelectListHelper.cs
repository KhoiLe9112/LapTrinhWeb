using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1080072.BusinessLayers;

namespace SV20T1080072.Web
{
	public class SelectListHelper
	{
		public static List<SelectListItem> Provinces()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			list.Add(new SelectListItem()
			{
				Value = "",
				Text = "-- Chọn tỉnh thành --"
			});

			foreach (var item in CommonDataService.ListOfProvinces())
				list.Add(new SelectListItem()
				{
					Value = item.ProvinceName,
					Text = item.ProvinceName
				});

			return list;
		}

        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Loại hàng --"
            });

            foreach (var item in CommonDataService.ListOfCategoryNames())
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });

            return list;
        }

        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Nhà cung cấp --"
            });

            foreach (var item in CommonDataService.ListOfSupplierNames())
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });

            return list;
        }

		public static List<SelectListItem> Customer()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			list.Add(new SelectListItem()
			{
				Value = "",
				Text = "-- Chọn khách hàng --"
			});

			foreach (var item in CommonDataService.ListOfCustomerNames())
				list.Add(new SelectListItem()
				{
					Value = item.CustomerName,
					Text = item.CustomerName
				});

			return list;
		}

		public static List<SelectListItem> Employee()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			list.Add(new SelectListItem()
			{
				Value = "",
				Text = "-- Chọn nhân viên --"
			});

			foreach (var item in CommonDataService.ListOfEmployeeNames())
				list.Add(new SelectListItem()
				{
					Value = item.FullName,
					Text = item.FullName
				});

			return list;
		}
	}
}
