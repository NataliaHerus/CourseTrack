using AutoMapper;
using BusinessLayer.Models;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<CourseWork, CourseWorkDto>().ReverseMap();
        }
    }
}
