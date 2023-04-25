using BusinessLayer.Models;
using BusinessLayer.Tasks;
using CourseTrack.Extensions;
using CourseTrack.Models;
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
            var result = new List<TaskViewModel>();

            foreach (var task in tasks)
                result.Add(new TaskViewModel() { Comment = task.Comment, Priority = task.Priority, Status = task.Status, CourseWorkId = task.CourseWorkId, Id = task.Id });

            return View("AllStudentTasks", result);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult AllStudentTasksById(int studentId, int courseWorkId)
        {
            var tasks = _taskFacade.GetStudentTasksById(studentId);
            ViewBag.StudentId = studentId;
            ViewBag.CourseWorkId = courseWorkId;

            var result = new List<TaskViewModel>();

            foreach (var task in tasks)
                result.Add(new TaskViewModel() { Comment = task.Comment, Priority = task.Priority, Status = task.Status, CourseWorkId = task.CourseWorkId, Id = task.Id });

            return View("AllStudentTasks", result);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public ActionResult Add(int id)
        {
            var task = new TaskViewModel();
            ViewBag.CourseWorkId = id;

            return View("AddEdit", task);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            var viewModel = new TaskViewModel() { Comment = task.Comment, Priority = task.Priority, Status = task.Status, CourseWorkId = task.CourseWorkId, Id = task.Id };
            ViewBag.TaskId = id;

            ViewData["readonly"] = User.GetRole() == Role.Student.ToString() ? "readonly" : "";
            ViewData["disabled"] = User.GetRole() == Role.Student.ToString() ? "disabled" : "";

            if (task == null)
                return NotFound();

            return View("AddEdit", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddEdit(TaskViewModel task)
        {
            var dto = new TaskDto() { Comment = task.Comment, Priority = task.Priority, Status = task.Status, CourseWorkId = task.CourseWorkId, Id = task.Id };

            if (ModelState.IsValid)
            {
                if (dto.Id == 0)
                    _taskFacade.AddTask(dto);
                else
                    _taskFacade.EditTask(dto);

                if (User.GetRole() == Role.Lecturer.ToString())
                    return RedirectToAction("GetLecturerStudents", "Student");
                else
                    return RedirectToAction("Details", "Student", new { id = dto.CourseWorkId });
            }

            return View(dto);
        }
    }
}
