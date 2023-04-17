using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CourseWorks
{
    public interface ICourseWorkFacade
    {
        Task<CourseWork> ChangeCourseWork(CourseWork courseWork);
        List<CourseWork> GetStudentCourseWorks(int studentId);
        Task<CourseWork> CreateCourseWorks(string theme, int studentId);
    }
}
