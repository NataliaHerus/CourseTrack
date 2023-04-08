using DataLayer.Enums;

namespace DataLayer.Account
{
    public interface IAccountRepository
    {
        bool UserExists(string email);

        void RegisterStudent(string firstName, string lastName, string email, string password);

        Role GetUserRole(string email);

        bool PasswordIsValid(string email, string password);

        string GetFullName(string email);
    }
}