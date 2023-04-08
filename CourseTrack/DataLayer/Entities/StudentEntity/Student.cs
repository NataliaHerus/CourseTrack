using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;

namespace DataLayer.Entities.StudentEntity
{
    public class Student
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public int? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }

        public ICollection<CourseWork>? CourseWorks { get; }
    }
}
