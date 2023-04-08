using System.ComponentModel;

namespace CourseTrack.Models
{
    public class RegisterViewModel
    {
        [DisplayName("Ім'я")]
        public string? FirstName { get; set; }
        [DisplayName("Прізвище")]
        public string? LastName { get; set; }
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Пароль")]
        public string? Password { get; set; }
        [DisplayName("Повторіть пароль")]
        public string ConfirmPassword { get; set; }
    }
}
