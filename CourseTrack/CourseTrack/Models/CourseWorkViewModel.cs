using DataLayer.Enums;

namespace CourseTrack.Models
{
    public class CourseWorkViewModel
    {
        public int? Id { get; set; }
        public string? Theme { get; set; }
        public Statuses Status { get; set; }
        public int? StudentId { get; set; }

        public int? LecturerId { get; set; }
    }
}
