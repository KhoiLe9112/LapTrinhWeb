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
                    Value = item.CategoryName,
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
                    Value = item.SupplierName,
                    Text = item.SupplierName
                });

            return list;
        }
    }
}
