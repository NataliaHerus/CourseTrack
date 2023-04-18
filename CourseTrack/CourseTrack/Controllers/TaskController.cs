using BusinessLayer.Tasks;
using CourseTrack.Extensions;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace CourseTrack.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskFacade _taskFacade;

        public TaskController(ITaskFacade taskFacade)
        {
            _taskFacade = taskFacade;
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult AllStudentTasks()
        {
            var tasks = _taskFacade.GetStudentTasksByEmail(User.GetEmail());
            return View("AllStudentTasks", tasks);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult AllStudentTasksById(int studentId, int courseWorkId)
        {
            var tasks = _taskFacade.GetStudentTasksById(studentId);
            ViewBag.StudentId = studentId;
            ViewBag.CourseWorkId = courseWorkId;

            return View("AllStudentTasks", tasks);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult Add(int id)
        {
            var task = new Task();
            ViewBag.CourseWorkId = id;

            return View("AddEdit", task);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            ViewBag.TaskId = id;

            ViewData["readonly"] = User.GetRole() == Role.Student.ToString() ? "readonly" : "";
            ViewData["disabled"] = User.GetRole() == Role.Student.ToString() ? "disabled" : "";

            if (task == null)
                return NotFound();

            return View("AddEdit", task);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddEdit(Task task)
        {
            if (ModelState.IsValid)
            {
                if (task.Id == 0)
                    _taskFacade.AddTask(task);
                else
                    _taskFacade.EditTask(task);

                if (User.GetRole() == Role.Lecturer.ToString())
                    return RedirectToAction("GetLecturerStudents", "Student");
                else
                    return RedirectToAction("Details", "Student", new { id = task.CourseWorkId });
            }

            return View(task);
        }
    }
}
