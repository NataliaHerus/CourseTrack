using BusinessLayer.Services;
using DataLayer.Account;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
using DataLayer.Lecturers;
using DataLayer.Students;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Account
{
    public class AccountFacade : IAccountFacade
    {
        private readonly IJwtService _jwtService;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public AccountFacade(IJwtService jwtService, IEmailService emailService, IStudentRepository studentRepository, ILecturerRepository lecturerRepository, IConfiguration configuration, IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _emailService = emailService;
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

        public void ForgotPassword(string email)
        {
            var student = _studentRepository.GetStudentByEmail(email);
            var lecturer = _lecturerRepository.GetLecturerByEmail(email);

            if (student == null && lecturer == null)
                throw new NullReferenceException("Користувач не знайдений");

            var token = student != null ? StudentForgotPasswordGenerateToken(student.Id) : LecturerForgotPasswordGenerateToken(lecturer.Id);

            var message = new Message(new string[] { email }, "Відновлення паролю",
                $"Для відновлення паролю перейдіть за цим посиланням:  {_configuration["MailerBaseURL"]}/account/setnewpassword?token={token}");

            _emailService.SendEmail(message);
        }

        public bool GetUserByToken(string token)
        {
            var currentDate = DateTime.UtcNow;

            var student = _studentRepository.GetStudentByToken(token, currentDate);
            var lecturer = _lecturerRepository.GetLecturerByToken(token, currentDate);

            return student != null || lecturer != null;
        }

        public void SetNewPassword(string token, string password)
        {
            var currentDate = DateTime.UtcNow;

            var student = _studentRepository.GetStudentByToken(token, currentDate);
            var lecturer = _lecturerRepository.GetLecturerByToken(token, currentDate);

            if (student != null)
                SetPasswordForStudent(student.Id, password);
            else
                SetPasswordForLecturer(lecturer.Id, password);
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

        private string StudentForgotPasswordGenerateToken(int id)
        {
            var token = "";
            var expirationDate = DateTime.UtcNow.AddDays(1);

            do
            {
                token = Guid.NewGuid().ToString();
            } while (_studentRepository.GetActiveTokens(expirationDate).Any(x => x == token));

            _studentRepository.SaveToken(id, token, expirationDate);
            return token;
        }

        private string LecturerForgotPasswordGenerateToken(int id)
        {
            var token = "";
            var expirationDate = DateTime.UtcNow;

            do
            {
                token = Guid.NewGuid().ToString();
            } while (_lecturerRepository.GetActiveTokens(expirationDate).Any(x => x == token));

            _lecturerRepository.SaveToken(id, token, expirationDate);
            return token;
        }

        private void SetPasswordForStudent(int id, string password)
        {
            var encryptionKey = _configuration["EncryptionKey"];

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(encryptionKey)))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                string hashPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                _studentRepository.SetPassword(id, hashPassword);
            }
        }

        private void SetPasswordForLecturer(int id, string password)
        {
            var encryptionKey = _configuration["EncryptionKey"];

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(encryptionKey)))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                string hashPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                _lecturerRepository.SetPassword(id, hashPassword);
            }
        }
    }
}
