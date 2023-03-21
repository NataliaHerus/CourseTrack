using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.TaskEntity
{
    public class Task
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public Statuses Status { get; set; }

        public Priorities Priority { get; set; }

        public int? CourseWorkId { get; set; }
        public CourseWork? CourseWork { get; set; }
    }
}
