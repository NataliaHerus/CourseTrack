using BusinessLayer.Models;
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
        Task<CourseWorkDto> ChangeCourseWork(CourseWorkDto courseWork);
        List<CourseWorkDto> GetStudentCourseWorks(int studentId);
        List<CourseWorkDto> GetStudentCourseWorksByEmail();
        Task<CourseWorkDto> CreateCourseWorks(string theme, int studentId);
    }
}
