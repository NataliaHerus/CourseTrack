using BusinessLayer.Services;
using DataLayer.Account;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Account
{
    public class AccountFacade : IAccountFacade
    {
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public AccountFacade(IJwtService jwtService, IConfiguration configuration, IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        public bool Register(string firstName, string lastName, string email, string password)
        {
            var encryptionKey = _configuration["EncryptionKey"];
            var userExists = _accountRepository.UserExists(email);

            if (userExists)
                throw new ArgumentException("Користувач з таким емейлом вже існує");

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(encryptionKey)))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                string hashPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                var student = new Student { FirstName = firstName, LastName = lastName, Email = email, Password = password };
                _accountRepository.RegisterStudent(student);
            }

            return true;
        }

        public bool Login(string email, string password)
        {
            var isValid = false;
            var encryptionKey = _configuration["EncryptionKey"];
            
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(encryptionKey)))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                string hashPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                var userRole = _accountRepository.UserIsStudent(email) ? Role.Student : Role.Lecturer;

                isValid = userRole == Role.Student ? _accountRepository.StudentPasswordIsValid(email, hashPassword)
                    : _accountRepository.LecturerPasswordIsValid(email, hashPassword);
            }

            return isValid;
        }

        public string GetToken(string email)
        {
            var userRole = _accountRepository.UserIsStudent(email) ? Role.Student : Role.Lecturer;
            var fullName = userRole == Role.Student ? _accountRepository.GetStudentFullName(email) : _accountRepository.GetLecturerFullName(email);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Name, email),
                    new Claim("FullName", $"{userRole} {fullName}"),
                    new Claim(ClaimTypes.Role, userRole.ToString())
                };

            var token = _jwtService.GenerateToken(claims);
            return token;
        }
    }
}
