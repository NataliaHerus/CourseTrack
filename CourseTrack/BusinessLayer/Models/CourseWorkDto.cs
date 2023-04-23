using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class CourseWorkDto
    {
        public int? Id { get; set; } 
        public string? Theme { get; set; }
        public Statuses Status { get; set; }
        public int? StudentId { get; set; }

        public int? LecturerId { get; set; }
    }
}
