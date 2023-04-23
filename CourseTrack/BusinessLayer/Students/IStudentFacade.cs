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
        StudentDto DeleteStudentFromLecturer(int id);
        StudentDto AddStudentToLecturer(int id);
        StudentDto DeleteStudent(int id);
        StudentDto GetStudentById(int id);
        List<StudentDto> GetAllStudentsList();

        List<StudentDto> GetLecturerStudentsList();
    }
}
