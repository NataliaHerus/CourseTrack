using DataLayer.Data;
using DataLayer.Entities.LecturerEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Lecturers
{
    public class LecturerRepository : ILecturerRepository
    {
        protected readonly CourseTrackDbContext _dbContext;
        public LecturerRepository(CourseTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Lecturer? GetLecturerByEmail(string email)
        {
            return _dbContext.Lecturers!.FirstOrDefault(x => x.Email == email);
        }
    }
}
