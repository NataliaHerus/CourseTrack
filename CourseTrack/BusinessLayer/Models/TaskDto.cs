using DataLayer.Enums;

namespace BusinessLayer.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public Statuses Status { get; set; }

        public Priorities Priority { get; set; }

        public int? CourseWorkId { get; set; }
        public CourseWorkDto? CourseWork { get; set; }
    }
}
