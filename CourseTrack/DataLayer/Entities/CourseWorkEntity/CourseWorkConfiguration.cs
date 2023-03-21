using DataLayer.Entities.LecturerEntity;
using DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.CourseWorkEntity
{
    public class CourseWorkConfiguration : IEntityTypeConfiguration<CourseWork>
    {
        public void Configure(EntityTypeBuilder<CourseWork>? builder)
        {
            if (builder != null)
            {
                builder
                .HasKey(x => x.Id);

                builder
                 .Property(x => x.Theme)
                 .HasMaxLength(100)
                 .IsRequired();

                builder
                .Property(x => x.Status).HasConversion<string>().IsRequired();

                builder
                    .HasOne(x => x.Lecturer)
                    .WithMany(x => x.CourseWorks)
                    .HasForeignKey(x => x.LecturerId);

                builder.
                     HasOne(b => b.Student)
                    .WithMany(i => i.CourseWorks)
                    .HasForeignKey(x => x.StudentId);
            }
        }
    }
}
