using DataLayer.Data;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;

namespace DataLayer.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CourseTrackDbContext _context;

        public AccountRepository(CourseTrackDbContext context)
        {
            _context = context;
        }

        public void RegisterStudent(string firstName, string lastName, string email, string password)
        {
            var student = new Student { FirstName = firstName, LastName = lastName, Email = email, Password = password };

            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public bool UserExists(string email)
        {
            return _context.Students.Any(s => s.Email == email) || _context.Lecturers.Any(s => s.Email == email);
        }

        public Role GetUserRole(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student != null)
            {
                return Role.Student;
            }

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Email == email);
            if (lecturer != null)
            {
                return Role.Lecturer;
            }

            throw new ArgumentException("Користувач не знайдений");
        }

        public bool PasswordIsValid(string email, string password)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student != null)
                return student.Password == password;

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Email == email);
            if (lecturer != null)
                return lecturer.Password == password;

            throw new ArgumentException("Користувач не знайдений");
        }

        public string GetFullName(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student != null)
                return $"{student.FirstName} {student.LastName}";

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Email == email);
            if (lecturer != null)
                return $"{lecturer.FirstName} {lecturer.LastName}";

            throw new ArgumentException("Користувач не знайдений");
        }
    }
}
