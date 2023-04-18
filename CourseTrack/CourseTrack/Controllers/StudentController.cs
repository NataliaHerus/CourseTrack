using BusinessLayer.Account;
using BusinessLayer.Students;
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

        public StudentController(IStudentFacade studentFacade)
        {
            _studentFacade = studentFacade;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Student student = _studentFacade.GetStudentById(id);
            return View(student);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult GetAllStudents()
        {
            IEnumerable<Student> students = _studentFacade.GetAllStudentsList();
            ViewBag.CurrentLecturer = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(students);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult GetLecturerStudents()
        {
            IEnumerable<Student> students = _studentFacade.GetLecturerStudentsList();
            return View(students);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(int id)
        {
            Student student = _studentFacade.GetStudentById(id);

            if (_studentFacade.GetLecturerStudentsList().Contains(student))
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
            Student student = _studentFacade.DeleteStudentFromLecturer(id);
            TempData["Message"] = "Студента " + student.LastName + " " + student.FirstName + " видалено зі списку ваших студентів";
            return RedirectToAction("GetAllStudents");
        }
    }
}
