using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace DataLayer.Entities.CourseWorkEntity
{
    public class CourseWork
    {
        public int Id { get; set; }
        public string? Theme { get; set; }
        public Statuses Status { get; set; }

        public int? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }

        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        public ICollection<Task>? Tasks { get; }
    }
}
