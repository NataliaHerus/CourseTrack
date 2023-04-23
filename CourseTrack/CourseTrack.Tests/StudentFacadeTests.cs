using AutoMapper;
using BusinessLayer.Models;
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
        private Mock<IMapper> _mapperMock;

        private IStudentFacade _studentFacade;

        [SetUp]
        public void Setup()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
            _courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapperMock = new Mock<IMapper>();

            _studentFacade = new StudentFacade(_studentRepositoryMock.Object, _httpContextAccessorMock.Object,
                _lecturerRepositoryMock.Object, _courseWorkRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public void DeleteStudentFromLecturer_WithValidId_ShouldSetLecturerIdToNullAndReturnStudent()
        {
            // Arrange
            int studentId = 1;
            var student = new Student { Id = studentId, LecturerId = 2 };
            _studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            var studentDto = new StudentDto { Id = studentId, LecturerId = null };
            _mapperMock.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);

            var service = new StudentFacade(_studentRepositoryMock.Object, null, null, null, _mapperMock.Object);

            // Act
            var result = _studentFacade.DeleteStudentFromLecturer(studentId);

            // Assert
            Assert.IsNull(result.LecturerId);
            _studentRepositoryMock.Verify(r => r.UpdateStudent(student), Times.Once);
            _studentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<StudentDto>(student), Times.Once);
            Assert.AreEqual(studentDto, result);
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

            _studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            _lecturerRepositoryMock.Setup(r => r.GetLecturerByEmail(lecturerEmail)).Returns(lecturer);

            _httpContextAccessorMock.Setup(a => a.HttpContext.User.Identity.Name).Returns(lecturerEmail);


            var studentDto = new StudentDto { Id = studentId };
            _mapperMock.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);

            // Act
            var result = _studentFacade.AddStudentToLecturer(studentId);

            // Assert
            Assert.AreEqual(lecturerId, student.LecturerId);
            _studentRepositoryMock.Verify(r => r.UpdateStudent(student), Times.Once);
            _studentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<StudentDto>(student), Times.Once);
            Assert.AreEqual(studentDto, result);
        }



        [Test]
        public void DeleteStudentFromLecturer_WithValidId_ShouldSetLecturerIdToNullAndReturnStudentDto()
        {
            // Arrange
            int studentId = 1;
            var student = new Student { Id = studentId, LecturerId = 2 };
            _studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);

            var studentDto = new StudentDto { Id = studentId };
            _mapperMock.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);


            // Act
            var result = _studentFacade.DeleteStudentFromLecturer(studentId);

            // Assert
            Assert.IsNull(result.LecturerId);
            _studentRepositoryMock.Verify(r => r.UpdateStudent(student), Times.Once);
            _studentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<StudentDto>(student), Times.Once);
            Assert.AreEqual(studentDto, result);
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

            var expectedStudents = new List<StudentDto>
    {
         new StudentDto { Id = 1, FirstName = "John Doe" },
        new StudentDto { Id = 2, FirstName = "Jane Smith" },
        new StudentDto { Id = 3, FirstName = "Bob Johnson" }
    };

            _mapperMock.Setup(m => m.Map<List<StudentDto>>(students)).Returns(expectedStudents);
            _studentRepositoryMock.Setup(r => r.GetAllStudentsList()).Returns(students);

            // Act
            var result = _studentFacade.GetAllStudentsList();

            // Assert
            Assert.IsInstanceOf<List<StudentDto>>(result);
            Assert.AreEqual(students.Count, result.Count);
            for (int i = 0; i < students.Count; i++)
            {
                Assert.AreEqual(students[i].Id, result[i].Id);
                Assert.AreEqual(students[i].FirstName, result[i].FirstName);
            }
        }

        [Test]
        public void GetStudentById_WithValidId_ShouldReturnStudentDto()
        {
            // Arrange
            var studentId = 1;
            var student = new Student { Id = studentId };

            _studentRepositoryMock.Setup(r => r.GetStudentById(studentId)).Returns(student);


            var studentDto = new StudentDto { Id = studentId };
            _mapperMock.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);


            // Act
            var result = _studentFacade.GetStudentById(studentId);

            // Assert
            _mapperMock.Verify(m => m.Map<StudentDto>(student), Times.Once);
            Assert.AreEqual(studentDto, result);
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

            var expectedStudents = new List<StudentDto>
    {
        new StudentDto { Id = 1, FirstName = "John Doe", LecturerId = lecturer.Id },
        new StudentDto { Id = 2, FirstName = "Jane Smith", LecturerId = lecturer.Id }
    };

            _mapperMock.Setup(m => m.Map<List<StudentDto>>(students)).Returns(expectedStudents);

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(u => u.Identity.Name).Returns(lecturerEmail);
            _httpContextAccessorMock.Setup(a => a.HttpContext.User).Returns(userMock.Object);

            _lecturerRepositoryMock.Setup(r => r.GetLecturerByEmail(lecturerEmail)).Returns(lecturer);

            _studentRepositoryMock.Setup(r => r.GetAllLecturerStudentsList(lecturer.Id)).Returns(students);


            // Act
            var result = _studentFacade.GetLecturerStudentsList();

            // Assert
            Assert.IsInstanceOf<List<StudentDto>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedStudents, result);
        }

    }
}
