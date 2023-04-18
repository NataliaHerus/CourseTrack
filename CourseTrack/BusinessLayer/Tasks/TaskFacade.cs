using DataLayer.Entities.StudentEntity;
using DataLayer.Tasks;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace BusinessLayer.Tasks
{
    public class TaskFacade : ITaskFacade
    {
        private readonly ITaskRepository _taskRepository;

        public TaskFacade(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<Task> GetStudentTasksById(int id)
        {
            return _taskRepository.GetStudentTasks(id);
        }

        public List<Task> GetStudentTasksByEmail(string email)
        {
            return _taskRepository.GetStudentTasks(email);
        }

        public void AddTask(Task task)
        {
            _taskRepository.AddTask(task);
        }

        public Task GetTaskById(int id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public void EditTask(Task task)
        {
            _taskRepository.EditTask(task);
        }
    }
}
