using DataLayer.Entities.StudentEntity;
using Task = DataLayer.Entities.TaskEntity.Task;

namespace BusinessLayer.Tasks
{
    public interface ITaskFacade
    {
        List<Task> GetStudentTasksById(int id);
        List<Task> GetStudentTasksByEmail(string email);
        void AddTask(Task task);
        Task GetTaskById(int id);
        void EditTask(Task task);
    }
}