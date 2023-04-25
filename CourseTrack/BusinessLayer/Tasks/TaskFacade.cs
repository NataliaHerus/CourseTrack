using AutoMapper;
using BusinessLayer.Models;
using DataLayer.Tasks;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace BusinessLayer.Tasks
{
    public class TaskFacade : ITaskFacade
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskFacade(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public List<TaskDto> GetStudentTasksById(int id)
        {
            var tasks = _taskRepository.GetStudentTasks(id);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public List<TaskDto> GetStudentTasksByEmail(string email)
        {
            var tasks = _taskRepository.GetStudentTasks(email);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public void AddTask(TaskDto task)
        {
            var entity = _mapper.Map<Task>(task);
            _taskRepository.AddTask(entity);
        }

        public TaskDto GetTaskById(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            return _mapper.Map<TaskDto>(task);
        }

        public void EditTask(TaskDto task)
        {
            var entity = _mapper.Map<Task>(task);
            _taskRepository.EditTask(entity);
        }
    }
}
