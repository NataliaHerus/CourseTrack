using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities.TaskEntity
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            if (builder != null)
            {
                builder
                .HasKey(x => x.Id);

                builder
                 .Property(x => x.Comment)
                 .HasMaxLength(1000);

                builder
                .Property(x => x.Status).HasConversion<string>().IsRequired();

                builder
               .Property(x => x.Priority).HasConversion<string>().IsRequired();

                builder
                   .HasOne(x => x.CourseWork)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.CourseWorkId);
            }
        }
    }
}
