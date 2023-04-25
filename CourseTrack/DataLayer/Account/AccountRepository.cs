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

        public void RegisterStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public bool UserExists(string email)
        {
            return _context.Students.Any(s => s.Email == email) || _context.Lecturers.Any(s => s.Email == email);
        }

        public bool UserIsStudent(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            return student != null;
        }

        public bool UserIsLecturer(string email)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(s => s.Email == email);
            return lecturer != null;
        }

        public bool StudentPasswordIsValid(string email, string password)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            return student.Password == password;
        }

        public bool LecturerPasswordIsValid(string email, string password)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(s => s.Email == email);
            return lecturer.Password == password;
        }

        public string GetStudentFullName(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            return $"{student.FirstName} {student.LastName}";
        }

        public string GetLecturerFullName(string email)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(s => s.Email == email);
            return $"{lecturer.FirstName} {lecturer.LastName}";
        }
    }
}
