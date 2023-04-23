using AutoMapper;
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
        protected readonly IMapper _mapper;

        public StudentFacade(IStudentRepository studentRepository, IHttpContextAccessor httpContextAccessor,
            ILecturerRepository lecturerRepository, ICourseWorkRepository courseWorkRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _courseWorkRepository = courseWorkRepository;
            _mapper = mapper;
        }
        public StudentDto DeleteStudentFromLecturer(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            student.LecturerId = null;
            _studentRepository.UpdateStudent(student);
            _studentRepository.SaveChangesAsync();
            return _mapper.Map<StudentDto>(student);
        }

        public StudentDto AddStudentToLecturer(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            Lecturer lecturer = _lecturerRepository.GetLecturerByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            student.LecturerId = lecturer.Id;
            _studentRepository.UpdateStudent(student);
            _studentRepository.SaveChangesAsync();
            return _mapper.Map<StudentDto>(student);
        }
        public StudentDto DeleteStudent(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            _studentRepository.DeleteStudent(student);
            return _mapper.Map<StudentDto>(student);
        }

        public List<StudentDto> GetAllStudentsList()
        {
            return _mapper.Map<List<StudentDto>>(_studentRepository.GetAllStudentsList());
        }

        public StudentDto GetStudentById(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            return _mapper.Map<StudentDto>(student);
        }

        public List<StudentDto> GetLecturerStudentsList()
        {
            Lecturer lecturer = _lecturerRepository.GetLecturerByEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            int id = lecturer.Id;
            List<Student> students = _studentRepository.GetAllLecturerStudentsList(id);
            return _mapper.Map<List<StudentDto>>(students);
        }
    }
}
