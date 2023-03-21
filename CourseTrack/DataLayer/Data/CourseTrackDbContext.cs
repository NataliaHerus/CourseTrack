using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Entities.TaskEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace DataLayer.Data
{
    public class CourseTrackDbContext : DbContext
    {
        public CourseTrackDbContext(DbContextOptions<CourseTrackDbContext> options)
            : base(options)
        {

        }

        public DbSet<CourseWork> CourseWorks { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.ApplyConfiguration(new CourseWorkConfiguration());
                modelBuilder.ApplyConfiguration(new LecturerConfiguration());
                modelBuilder.ApplyConfiguration(new StudentConfiguration());
                modelBuilder.ApplyConfiguration(new TaskConfiguration());
            }
        }
    }
}
