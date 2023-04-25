using DataLayer.Enums;

namespace CourseTrack.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public Statuses Status { get; set; }
        public Priorities Priority { get; set; }

        public int? CourseWorkId { get; set; }
    }
}
