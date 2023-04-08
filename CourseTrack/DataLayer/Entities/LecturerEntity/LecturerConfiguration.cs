using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Entities.LecturerEntity
{
    public class LecturerConfiguration : IEntityTypeConfiguration<Lecturer>
    {
        public void Configure(EntityTypeBuilder<Lecturer> builder)
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
            }
        }
    }
}
