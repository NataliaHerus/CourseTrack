using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;

namespace DataLayer.Account
{
    public interface IAccountRepository
    {
        bool UserExists(string email);

        void RegisterStudent(Student student);

        bool UserIsStudent(string email);

        bool UserIsLecturer(string email);

        string GetStudentFullName(string email);

        string GetLecturerFullName(string email);

        bool StudentPasswordIsValid(string email, string password);

        bool LecturerPasswordIsValid(string email, string password);
    }
}