using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;

namespace DataLayer.Entities.LecturerEntity
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpirationDate { get; set; }

        public ICollection<Student>? Students { get; }

        public ICollection<CourseWork>? CourseWorks { get; }
    }
}
