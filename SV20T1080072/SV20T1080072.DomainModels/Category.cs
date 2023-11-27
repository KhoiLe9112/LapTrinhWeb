using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080072.DomainModels
{
	/// <summary>
	/// 
	/// </summary>
	public class Category
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
