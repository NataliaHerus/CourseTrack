using BusinessLayer.Models;
using DataLayer.Entities;
using DataLayer.Entities.StudentEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Students
{
    public interface IStudentFacade
    {
        Student DeleteStudentFromLecturer(int id);
        Student AddStudentToLecturer(int id);
        Student DeleteStudent(int id);
        Student GetStudentById(int id);
        List<Student> GetAllStudentsList();

        List<Student> GetLecturerStudentsList();
    }
}
