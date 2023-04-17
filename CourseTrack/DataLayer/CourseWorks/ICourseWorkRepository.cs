using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CourseWorks
{
    public interface ICourseWorkRepository
    {
        List<CourseWork> GetAllStudentCourseWorks(int studentId);
        CourseWork CreateCourseWorks(CourseWork courseWork);
        CourseWork GetCourseWorkById(int id);
        CourseWork UpdateCourseWork(CourseWork courseWork);

        Task<int> SaveChangesAcync();
    }
}
