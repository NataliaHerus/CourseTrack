using DataLayer.CourseWorks;
using DataLayer.Data;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using DataLayer.Lecturers;
using DataLayer.Students;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CourseWorks
{
    public class CourseWorkFacade : ICourseWorkFacade
    {
        private readonly ICourseWorkRepository _courseWorkRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILecturerRepository _lecturerRepository;

        public CourseWorkFacade(ICourseWorkRepository courseWorkRepository, IHttpContextAccessor httpContextAccessor,
             ILecturerRepository lecturerRepository, IStudentRepository studentRepository)
        {
            _courseWorkRepository = courseWorkRepository;
            _httpContextAccessor = httpContextAccessor;
            _lecturerRepository = lecturerRepository;
            _studentRepository = studentRepository;
        }

        public async Task<CourseWork> ChangeCourseWork(CourseWork courseWork)
        {
            var updatedCourseWork = _courseWorkRepository.GetCourseWorkById(courseWork.Id);
            updatedCourseWork.Status = courseWork.Status;
            updatedCourseWork.Theme = courseWork.Theme;
            _courseWorkRepository.UpdateCourseWork(updatedCourseWork);
            await _courseWorkRepository.SaveChangesAcync();
            return updatedCourseWork;
        }

        public async Task<CourseWork> CreateCourseWorks(string theme, int studentId)
        {
            Lecturer lecturer = _lecturerRepository.GetLecturerByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            var newCourseWork = new CourseWork() { Theme = theme, StudentId = studentId, Status = Statuses.New, LecturerId = lecturer.Id };
            _courseWorkRepository.CreateCourseWorks(newCourseWork);
            await _courseWorkRepository.SaveChangesAcync();

            return newCourseWork;
        }

        public List<CourseWork> GetStudentCourseWorks(int studentId)
        {
            return _courseWorkRepository.GetAllStudentCourseWorks(studentId);
        }

        public List<CourseWork> GetStudentCourseWorksByEmail()
        {
            Student student = _studentRepository.GetStudentByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            return _courseWorkRepository.GetAllStudentCourseWorksByEmail(student.Email);
        }
    }
}
