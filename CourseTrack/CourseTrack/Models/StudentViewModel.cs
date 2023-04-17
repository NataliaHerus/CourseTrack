using System.ComponentModel;

namespace CourseTrack.Models
{
    public class StudentViewModel
    {
        [DisplayName("Ім'я")]
        public string? FirstName { get; set; }

        [DisplayName("Прізвище")]
        public string? LastName { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }
    }
}
