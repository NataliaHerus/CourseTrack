using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Students
{
    public interface IStudentRepository
    {
        Student UpdateStudent(Student student);

        Task<Student> DeleteStudent(Student student);
        List<Student> GetAllStudentsList();

        List<Student> GetAllLecturerStudentsList(int lecturerId);
        Student? GetStudentById(int id);
        Task<int> SaveChangesAsync();

        Student? GetStudentByEmail(string email);
    }
}
