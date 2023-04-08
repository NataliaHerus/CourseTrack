using System.ComponentModel;

namespace CourseTrack.Models
{
    public class LoginViewModel
    {
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Пароль")]
        public string? Password { get; set; }
    }
}
