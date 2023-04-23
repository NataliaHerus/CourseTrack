using System.ComponentModel;

namespace CourseTrack.Models
{
    public class StudentViewModel
    {
        [DisplayName("Номер")]
        public string? Id { get; set; }

        [DisplayName("Ім'я")]
        public string? FirstName { get; set; }

        [DisplayName("Прізвище")]
        public string? LastName { get; set; }

        [DisplayName("По батькові")]
        public string? MiddleName{ get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }
    }
}
