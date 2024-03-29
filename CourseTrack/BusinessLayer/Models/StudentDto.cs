﻿using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class StudentDto
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpirationDate { get; set; }

        public int? LecturerId { get; set; }
    }
}
