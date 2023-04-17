using DataLayer.Data;
using DataLayer.Entities.StudentEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await Task.Run(() => _dbContext.Students!.Remove(student));
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

        public Student? GetStudentById(int id)
        {
            return _dbContext.Students!.FirstOrDefault(x => x.Id == id);
        }
        public async Task<int> SaveChangesAcync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public Student UpdateStudent(Student student)
        {
            _dbContext.Entry(student!).State = EntityState.Modified;
            return student;
        }
    }
}
