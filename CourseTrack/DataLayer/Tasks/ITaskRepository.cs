using Task = DataLayer.Entities.TaskEntity.Task;

namespace DataLayer.Tasks
{
    public interface ITaskRepository
    {
        List<Task> GetStudentTasks(int id);
        List<Task> GetStudentTasks(string email);
        void AddTask(Task task);
        Task GetTaskById(int id);
        void EditTask(Task task);
    }
}