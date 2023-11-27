using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace SV20T1080072.Web.Models
{
	public class PersonDAL
	{
		public List<Person> List()
		{
			List<Person> list = new List<Person>(); //ten bien: camelCase
			list.Add(new Person()
			{
				PersonId = 1,
				Name = "Le Minh Khoi",
				Address = "31 Ngo Quyen",
				Email = "khoile@gmail.com"
			});

			list.Add(new Person()
			{
				PersonId = 2,
				Name = "Le Minh Hieu",
				Address = "31 Ngo Quyen",
				Email = "khoile@gmail.com"
			});

			list.Add(new Person()
			{
				PersonId = 1,
				Name = "Le Minh Khoa",
				Address = "31 Ngo Quyen",
				Email = "khoile@gmail.com"
			});

			return list;
		}

	}
}
