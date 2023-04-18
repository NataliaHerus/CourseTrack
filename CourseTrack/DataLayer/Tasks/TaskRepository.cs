using DataLayer.Data;
using Microsoft.EntityFrameworkCore;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace DataLayer.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        protected readonly CourseTrackDbContext _dbContext;
        public TaskRepository(CourseTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Task> GetStudentTasks(int id)
        {
            return _dbContext
                .Tasks
                .Include(t => t.CourseWork)
                .Where(x => x.CourseWork!.StudentId == id)
                .ToList();
        }

        public List<Task> GetStudentTasks(string email)
        {
            return _dbContext
                .Tasks
                .Include(t => t.CourseWork)
                .Where(x => x.CourseWork!.Student!.Email == email)
                .ToList();
        }

        public void AddTask(Task task)
        {
            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();
        }

        public void EditTask(Task task)
        {
            var entity = _dbContext.Tasks.FirstOrDefault(t => t.Id == task.Id);

            entity.Comment = task.Comment;
            entity.Status = task.Status;
            entity.Priority = task.Priority;

            _dbContext.SaveChanges();
        }

        public Task GetTaskById(int id)
        {
            var task = _dbContext.Tasks.FirstOrDefault(t => t.Id == id);

            return task;
        }
    }
}
