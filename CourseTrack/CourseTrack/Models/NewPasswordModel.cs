using System.ComponentModel;

namespace CourseTrack.Models
{
    public class NewPasswordModel
    {
        [DisplayName("Пароль")]
        public string? Password { get; set; }

        [DisplayName("Повторіть пароль")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
