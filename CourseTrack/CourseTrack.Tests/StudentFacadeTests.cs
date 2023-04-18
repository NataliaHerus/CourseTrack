using BusinessLayer.Students;
using DataLayer.CourseWorks;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Lecturers;
using DataLayer.Students;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseTrack.Tests
{
    public class StudentFacadeTests
    {
        private Mock<IStudentRepository> _studentRepositoryMock;
        private Mock<ILecturerRepository> _lecturerRepositoryMock;
        private Mock<ICourseWorkRepository> _courseWorkRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private IStudentFacade _studentFacade;

        [SetUp]
        public void Setup()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
            _courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _studentFacade = new StudentFacade(_studentRepositoryMock.Object, _httpContextAccessorMock.Object,
                _lecturerRepositoryMock.Object, _courseWorkRepositoryMock.Object);
        }

        [Test]
        public void DeleteStudentFromLecturer_WithValidId_ShouldSetLecturerIdToNullAndReturnStudent()
        {
            // Arrange
            int studentId = 1;
            var student = new Student { Id = studentId, LecturerId = 2 };
            _studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            // Act
            var result = _studentFacade.DeleteStudentFromLecturer(studentId);

            // Assert
            Assert.AreEqual(null, result.LecturerId);
            _studentRepositoryMock.Verify(r => r.UpdateStudent(student), Times.Once);
            _studentRepositoryMock.Verify(r => r.SaveChangesAcync(), Times.Once);
        }

        [Test]
        public void AddStudentToLecturer_ShouldSetLecturerIdAndUpdateStudent()
        {
            // Arrange
            var studentId = 1;
            var lecturerEmail = "lecturer@example.com";
            var lecturerId = 10;

            var student = new Student { Id = studentId };
            var lecturer = new Lecturer { Id = lecturerId, Email = lecturerEmail };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            var lecturerRepositoryMock = new Mock<ILecturerRepository>();
            lecturerRepositoryMock.Setup(r => r.GetLecturerByEmail(lecturerEmail)).Returns(lecturer);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext.User.Identity.Name).Returns(lecturerEmail);

            var service = new StudentFacade(studentRepositoryMock.Object, httpContextAccessorMock.Object, lecturerRepositoryMock.Object, null);

            // Act
            var result = service.AddStudentToLecturer(studentId);

            // Assert
            Assert.AreEqual(lecturerId, student.LecturerId);
            studentRepositoryMock.Verify(r => r.UpdateStudent(student), Times.Once);
            studentRepositoryMock.Verify(r => r.SaveChangesAcync(), Times.Once);
            Assert.AreEqual(student, result);
        }


        [Test]
        public void DeleteStudent_WithValidId_ShouldDeleteStudentAndReturnIt()
        {
            // Arrange
            var studentId = 1;
            var student = new Student { Id = studentId };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            var service = new StudentFacade(studentRepositoryMock.Object, null, null, null);

            // Act
            var result = service.DeleteStudent(studentId);

            // Assert
            studentRepositoryMock.Verify(r => r.DeleteStudent(student), Times.Once);
            Assert.AreEqual(student, result);
        }

        [Test]
        public void GetAllStudentsList_ShouldReturnListOfStudents()
        {
            // Arrange
            var students = new List<Student>
    {
        new Student { Id = 1, FirstName = "John Doe" },
        new Student { Id = 2, FirstName = "Jane Smith" },
        new Student { Id = 3, FirstName = "Bob Johnson" }
    };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(r => r.GetAllStudentsList()).Returns(students);

            var service = new StudentFacade(studentRepositoryMock.Object, null, null, null);

            // Act
            var result = service.GetAllStudentsList();

            // Assert
            Assert.IsInstanceOf<List<Student>>(result);
            Assert.AreEqual(students.Count, result.Count);
            for (int i = 0; i < students.Count; i++)
            {
                Assert.AreEqual(students[i].Id, result[i].Id);
                Assert.AreEqual(students[i].FirstName, result[i].FirstName);
            }
        }

        [Test]
        public void GetStudentById_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentId = 1;
            var student = new Student { Id = studentId };

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            var service = new StudentFacade(studentRepositoryMock.Object, null, null, null);

            // Act
            var result = service.GetStudentById(studentId);

            // Assert
            Assert.AreEqual(student, result);
        }

        [Test]
        public void GetLecturerStudentsList_ShouldReturnListOfStudents()
        {
            // Arrange
            var lecturerEmail = "lecturer@example.com";
            var lecturer = new Lecturer { Id = 1, Email = lecturerEmail };
            var students = new List<Student>
    {
        new Student { Id = 1, FirstName = "John Doe", LecturerId = lecturer.Id },
        new Student { Id = 2, FirstName = "Jane Smith", LecturerId = lecturer.Id },
        new Student { Id = 3, FirstName = "Bob Johnson", LecturerId = 2 }
    };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(u => u.Identity.Name).Returns(lecturerEmail);
            httpContextAccessorMock.Setup(a => a.HttpContext.User).Returns(userMock.Object);

            var lecturerRepositoryMock = new Mock<ILecturerRepository>();
            lecturerRepositoryMock.Setup(r => r.GetLecturerByEmail(lecturerEmail)).Returns(lecturer);

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(r => r.GetAllLecturerStudentsList(lecturer.Id)).Returns(students);

            var service = new StudentFacade(studentRepositoryMock.Object, httpContextAccessorMock.Object, lecturerRepositoryMock.Object, null);

            // Act
            var result = service.GetLecturerStudentsList();

            // Assert
            Assert.IsInstanceOf<List<Student>>(result);
            Assert.AreEqual(3, result.Count);
        }


    }
}
