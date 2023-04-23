using AutoMapper;
using BusinessLayer.Account;
using BusinessLayer.Models;
using BusinessLayer.Students;
using CourseTrack.Models;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTrack.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentFacade _studentFacade;
        private readonly IMapper _mapper;

        public StudentController(IStudentFacade studentFacade, IMapper mapper)
        {
            _studentFacade = studentFacade;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            StudentDto student = _studentFacade.GetStudentById(id);
            return View(_mapper.Map<StudentViewModel>(student));
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult GetAllStudents()
        {
            IEnumerable<StudentDto> students = _studentFacade.GetAllStudentsList();
            ViewBag.CurrentLecturer = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(_mapper.Map<IEnumerable<StudentViewModel>>(students));
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult GetLecturerStudents()
        {
            IEnumerable<StudentDto> students = _studentFacade.GetLecturerStudentsList();
            return View(_mapper.Map<IEnumerable<StudentViewModel>>(students));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(int id)
        {
            StudentDto student = _studentFacade.GetStudentById(id);
            List<StudentDto> students = _studentFacade.GetLecturerStudentsList();
            bool cont = students.Any(s => s.Id == student.Id);
            if (cont)
            {
                TempData["Message"] = "Студент " + student.LastName + " " + student.FirstName + " вже є в списку ваших студентів";
            }
            else
            {
                _studentFacade.AddStudentToLecturer(id);
                TempData["Message"] = "Студента " + student.LastName + " " + student.FirstName + " додано до списку ваших студентів";
            }

            return RedirectToAction("GetAllStudents");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudent(int id)
        {
            StudentDto student = _studentFacade.DeleteStudentFromLecturer(id);
            TempData["Message"] = "Студента " + student.LastName + " " + student.FirstName + " видалено зі списку ваших студентів";
            return RedirectToAction("GetAllStudents");
        }
    }
}
