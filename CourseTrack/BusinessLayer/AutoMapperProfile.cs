using AutoMapper;
using BusinessLayer.Models;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.StudentEntity;
using Task = DataLayer.Entities.TaskEntity.Task;

namespace BusinessLayer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Task, TaskDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap()
                .ForPath(_ => _.Token, dest => dest.Ignore())
                .ForPath(_ => _.TokenExpirationDate, dest => dest.Ignore());

            CreateMap<CourseWork, CourseWorkDto>().ReverseMap();
        }
    }
}
