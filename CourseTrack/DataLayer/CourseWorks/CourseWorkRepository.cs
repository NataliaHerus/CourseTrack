using DataLayer.Data;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CourseWorks
{
    public class CourseWorkRepository: ICourseWorkRepository
    {
        protected readonly CourseTrackDbContext _dbContext;
        public CourseWorkRepository(CourseTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseWork CreateCourseWorks(CourseWork courseWork)
        {
            CourseWork newCourseWork = new CourseWork { StudentId = courseWork.StudentId, Theme = courseWork.Theme, 
                LecturerId = courseWork.LecturerId, Status = courseWork.Status };
            _dbContext.CourseWorks.Add(newCourseWork);
            _dbContext.SaveChanges();
            return newCourseWork;
        }

        public List<CourseWork> GetAllStudentCourseWorks(int studentId)
        {
            return _dbContext.CourseWorks.Where(x => x.StudentId == studentId).ToList();
        }

        public CourseWork GetCourseWorkById(int? id)
        {
            return _dbContext.CourseWorks!.FirstOrDefault(x => x.Id == id);
        }

        public CourseWork UpdateCourseWork(CourseWork courseWork)
        {
            _dbContext.Entry(courseWork!).State = EntityState.Modified;
            return courseWork;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public List<CourseWork> GetAllStudentCourseWorksByEmail(string email)
        {
            return _dbContext.CourseWorks.Where(x => x.Student.Email == email).ToList();
        }
    }
}
