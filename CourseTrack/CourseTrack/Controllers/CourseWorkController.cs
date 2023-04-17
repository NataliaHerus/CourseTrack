using BusinessLayer.CourseWorks;
using BusinessLayer.Students;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using Microsoft.AspNetCore.Mvc;

namespace CourseTrack.Controllers
{
    public class CourseWorkController : Controller
    {
        private readonly ICourseWorkFacade _courseWorkFacade;

        public CourseWorkController(ICourseWorkFacade courseWorkFacade)
        {
            _courseWorkFacade = courseWorkFacade;
        }

        [HttpGet]
        public ActionResult GetStudentCoursWorks([FromRoute] int Id)
        {
            IEnumerable<CourseWork> courseWorks = _courseWorkFacade.GetStudentCourseWorks(Id);
            ViewBag.Id = Id;
            return View(courseWorks);
        }

        [HttpGet]
        public ActionResult GetStudentViewCourseWorks()
        {
            IEnumerable<CourseWork> courseWorks = _courseWorkFacade.GetStudentCourseWorksByEmail();
            return View(courseWorks);
        }

        [HttpPost]
        public ActionResult UpdateCourseWork(CourseWork courseWork)
        {
            _courseWorkFacade.ChangeCourseWork(courseWork);
            TempData["Message"] = "Курсову роботу відредаговано";
            return RedirectToAction("GetStudentCoursWorks", new { Id = courseWork.StudentId });
        }

        [HttpPost]
        public ActionResult CreateCourseWorks(int studentId, string theme)
        {
            _courseWorkFacade.CreateCourseWorks(theme, studentId);
            TempData["Message"] = "Курсову роботу створено";
            return RedirectToAction("GetStudentCoursWorks", new { Id = studentId });
        }
    }
}
