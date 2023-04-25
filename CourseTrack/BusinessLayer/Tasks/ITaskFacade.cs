using BusinessLayer.Models;

namespace BusinessLayer.Tasks
{
    public interface ITaskFacade
    {
        List<TaskDto> GetStudentTasksById(int id);
        List<TaskDto> GetStudentTasksByEmail(string email);
        void AddTask(TaskDto task);
        TaskDto GetTaskById(int id);
        void EditTask(TaskDto task);
    }
}