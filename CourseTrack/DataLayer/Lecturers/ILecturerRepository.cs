using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Lecturers
{
    public interface ILecturerRepository
    {
        Lecturer? GetLecturerByEmail(string email);
    }
}
