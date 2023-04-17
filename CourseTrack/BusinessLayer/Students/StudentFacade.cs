using BusinessLayer.Models;
using BusinessLayer.Services;
using DataLayer.Account;
using DataLayer.CourseWorks;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Lecturers;
using DataLayer.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Students
{
    public class StudentFacade : IStudentFacade
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly ICourseWorkRepository _courseWorkRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentFacade(IStudentRepository studentRepository, IHttpContextAccessor httpContextAccessor,
            ILecturerRepository lecturerRepository, ICourseWorkRepository courseWorkRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _courseWorkRepository = courseWorkRepository;
        }
        public Student DeleteStudentFromLecturer(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            student.LecturerId = null;
            _studentRepository.UpdateStudent(student);
            _studentRepository.SaveChangesAcync();
            return student;
        }

        public Student AddStudentToLecturer(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            Lecturer lecturer = _lecturerRepository.GetLecturerByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            student.LecturerId = lecturer.Id;
            _studentRepository.UpdateStudent(student);
            _studentRepository.SaveChangesAcync();
            return student;
        }
        public Student DeleteStudent(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            _studentRepository.DeleteStudent(student);
            return student;
        }

        public List<Student> GetAllStudentsList()
        {
            return _studentRepository.GetAllStudentsList();
        }

        public Student GetStudentById(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            return student;
        }

        public List<Student> GetLecturerStudentsList()
        {
            Lecturer lecturer = _lecturerRepository.GetLecturerByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            int id = lecturer.Id;
            return _studentRepository.GetAllLecturerStudentsList(id);
        }
    }
}
