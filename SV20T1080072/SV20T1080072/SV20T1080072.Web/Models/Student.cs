namespace SV20T1080072.Web.Models
{
	public class Student
	{
		public string? StudentId { get;	set;}
		public string? StudentName { get; set; }
	}

	public class StudentDAL
	{
		public List<Student> List()
		{
			List<Student> students = new List<Student>();

			students.Add(new Student
			{
				StudentId = "20T1080072",
				StudentName = "Lê Minh Khôi",
			});
			students.Add(new Student
			{
				StudentId = "20T1080073",
				StudentName = "Lê Minh Phong",
			});

			return students;
		}
	}
}
