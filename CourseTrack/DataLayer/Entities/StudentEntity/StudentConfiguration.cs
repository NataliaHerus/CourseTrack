using DataLayer.Entities.LecturerEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.StudentEntity
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            if (builder != null)
            {
                builder
                    .HasKey(x => x.Id);

                builder
                 .Property(x => x.FirstName)
                 .HasMaxLength(50)
                 .IsRequired();

                builder
                 .Property(x => x.LastName)
                 .HasMaxLength(50)
                 .IsRequired();

                builder
                .Property(x => x.MiddleName)
                .HasMaxLength(50);

                builder
                 .HasIndex(i => i.Email)
                    .IsUnique();

                builder
                 .Property(x => x.Password).IsRequired();

                builder
                   .HasOne(x => x.Lecturer)
                   .WithMany(x => x.Students)
                   .HasForeignKey(x => x.LecturerId);
            }
        }
    }
}
