using DataLayer.Data;
using DataLayer.Entities.StudentEntity;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Students
{
    public class StudentRepository : IStudentRepository
    {
        protected readonly CourseTrackDbContext _dbContext;
        public StudentRepository(CourseTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> DeleteStudent(Student student)
        {
            await System.Threading.Tasks.Task.Run(() => _dbContext.Students!.Remove(student));
            return student;
        }

        public List<Student> GetAllLecturerStudentsList(int lecturerId)
        {
            return _dbContext.Students.Where(x => x.LecturerId == lecturerId).ToList();
        }

        public List<Student> GetAllStudentsList()
        {
            return _dbContext.Students!.ToList();
        }

        public Student? GetStudentByEmail(string email)
        {
            return _dbContext.Students!.FirstOrDefault(x => x.Email == email);
        }

        public Student? GetStudentById(int id)
        {
            return _dbContext.Students!.FirstOrDefault(x => x.Id == id);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public Student UpdateStudent(Student student)
        {
            _dbContext.Entry(student!).State = EntityState.Modified;
            return student;
        }

        public IEnumerable<string> GetActiveTokens(DateTime expirationDate)
        {
            var tokens = _dbContext
                .Students
                .Where(s => s.TokenExpirationDate > expirationDate)
                .Select(s => s.Token);

            return tokens;
        }

        public void SaveToken(int id, string token, DateTime tokenExpirationDate)
        {
            var student = _dbContext.Students.First(x => x.Id == id);
            student.Token = token;
            student.TokenExpirationDate = tokenExpirationDate;

            _dbContext.SaveChanges();
        }

        public void SetPassword(int id, string password)
        {
            var student = _dbContext.Students.First(x => x.Id == id);
            student.Password = password;
            student.Token = null;

            _dbContext.SaveChanges();
        }

        public Student GetStudentByToken(string token, DateTime expirationDate)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Token == token && s.TokenExpirationDate > expirationDate);
            return student;
        }
    }
}
