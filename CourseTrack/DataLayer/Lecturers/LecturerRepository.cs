using DataLayer.Data;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Lecturers
{
    public class LecturerRepository : ILecturerRepository
    {
        protected readonly CourseTrackDbContext _dbContext;
        public LecturerRepository(CourseTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Lecturer? GetLecturerByEmail(string email)
        {
            return _dbContext.Lecturers!.FirstOrDefault(x => x.Email == email);
        }

        public IEnumerable<string> GetActiveTokens(DateTime expirationDate)
        {
            var tokens = _dbContext
                .Lecturers
                .Where(s => s.TokenExpirationDate > expirationDate)
                .Select(s => s.Token);

            return tokens;
        }

        public void SaveToken(int id, string token, DateTime tokenExpirationDate)
        {
            var lecturer = _dbContext.Lecturers.First(x => x.Id == id);
            lecturer.Token = token;
            lecturer.TokenExpirationDate = tokenExpirationDate;

            _dbContext.SaveChanges();
        }

        public void SetPassword(int id, string password)
        {
            var lecturer = _dbContext.Lecturers.First(x => x.Id == id);
            lecturer.Password = password;
            lecturer.Token = null;

            _dbContext.SaveChanges();
        }

        public Lecturer GetLecturerByToken(string token, DateTime expirationDate)
        {
            var lecturer = _dbContext.Lecturers.FirstOrDefault(s => s.Token == token && s.TokenExpirationDate > expirationDate);
            return lecturer;
        }
    }
}
