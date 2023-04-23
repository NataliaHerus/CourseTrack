using AutoMapper;
using BusinessLayer.CourseWorks;
using BusinessLayer.Models;
using BusinessLayer.Students;
using CourseTrack.Models;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTrack.Controllers
{
    [Authorize]
    public class CourseWorkController : Controller
    {
        private readonly ICourseWorkFacade _courseWorkFacade;
        private readonly IMapper _mapper;

        public CourseWorkController(ICourseWorkFacade courseWorkFacade, IMapper mapper)
        {
            _courseWorkFacade = courseWorkFacade;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetStudentCoursWorks([FromRoute] int Id)
        {
            IEnumerable<CourseWorkDto> courseWorks = _courseWorkFacade.GetStudentCourseWorks(Id);
            ViewBag.Id = Id;
            return View(_mapper.Map<IEnumerable<CourseWorkViewModel>>(courseWorks));
        }

        [HttpGet]
        public ActionResult GetStudentViewCourseWorks()
        {
            IEnumerable<CourseWorkDto> courseWorks = _courseWorkFacade.GetStudentCourseWorksByEmail();
            return View(_mapper.Map<IEnumerable<CourseWorkViewModel>>(courseWorks));
        }

        [HttpPost]
        public ActionResult UpdateCourseWork(CourseWorkViewModel courseWork)
        {
            CourseWorkDto courseWorkToUpdate = _mapper.Map<CourseWorkDto>(courseWork);
            _courseWorkFacade.ChangeCourseWork(courseWorkToUpdate);
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
