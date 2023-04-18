using BusinessLayer.CourseWorks;
using BusinessLayer.Students;
using DataLayer.CourseWorks;
using DataLayer.Entities.CourseWorkEntity;
using DataLayer.Entities.LecturerEntity;
using DataLayer.Entities.StudentEntity;
using DataLayer.Enums;
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
    public class CourseWorkFacadeTests
    {
        private Mock<IStudentRepository> _studentkRepositoryMock;
        private Mock<ILecturerRepository> _lecturerRepositoryMock;
        private Mock<ICourseWorkRepository> _courseWorkRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private ICourseWorkFacade _courceWorkFacade;

        [SetUp]
        public void Setup()
        {
            _studentkRepositoryMock = new Mock<IStudentRepository>();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
            _courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _courceWorkFacade = new CourseWorkFacade(_courseWorkRepositoryMock.Object, _httpContextAccessorMock.Object,
                _lecturerRepositoryMock.Object, _studentkRepositoryMock.Object);
        }

        [Test]
        public async Task ChangeCourseWork_UpdatesCourseWorkAndReturnsUpdatedCourseWork()
        {
            // Arrange
            var courseWork = new CourseWork { Id = 1, Status = Statuses.InProgress, Theme = "Math" };
            var updatedCourseWork = new CourseWork { Id = 1, Status = Statuses.Done, Theme = "History" };
            var courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            courseWorkRepositoryMock.Setup(x => x.GetCourseWorkById(courseWork.Id)).Returns(updatedCourseWork);
            var service = new CourseWorkFacade(courseWorkRepositoryMock.Object, null, null, null);

            // Act
            var result = await service.ChangeCourseWork(courseWork);

            // Assert
            Assert.AreEqual(updatedCourseWork.Id, result.Id);
            Assert.AreEqual(updatedCourseWork.Status, result.Status);
            Assert.AreEqual(updatedCourseWork.Theme, result.Theme);
            courseWorkRepositoryMock.Verify(x => x.UpdateCourseWork(updatedCourseWork), Times.Once);
            courseWorkRepositoryMock.Verify(x => x.SaveChangesAcync(), Times.Once);
        }

        [Test]
        public async Task CreateCourseWorks_CreatesAndReturnsNewCourseWork()
        {
            // Arrange
            var theme = "Math";
            var studentId = 1;
            var lecturerId = 2;
            var newCourseWork = new CourseWork {Id = 1, Theme = theme, StudentId = studentId, Status = Statuses.New, LecturerId = lecturerId };
            var lecturerRepositoryMock = new Mock<ILecturerRepository>();
            lecturerRepositoryMock.Setup(x => x.GetLecturerByEmail(It.IsAny<string>())).Returns(new Lecturer { Id = lecturerId });
            var courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            courseWorkRepositoryMock.Setup(x => x.CreateCourseWorks(It.IsAny<CourseWork>())).Callback((CourseWork c) => c.Id = 1);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@example.com");
            var service = new CourseWorkFacade(courseWorkRepositoryMock.Object, httpContextAccessorMock.Object, lecturerRepositoryMock.Object, null);

            // Act
            var result = await service.CreateCourseWorks(theme, studentId);

            // Assert
            Assert.AreEqual(newCourseWork.Id, result.Id);
            Assert.AreEqual(newCourseWork.Theme, result.Theme);
            Assert.AreEqual(newCourseWork.StudentId, result.StudentId);
            Assert.AreEqual(newCourseWork.Status, result.Status);
            Assert.AreEqual(newCourseWork.LecturerId, result.LecturerId);
            courseWorkRepositoryMock.Verify(x => x.CreateCourseWorks(It.IsAny<CourseWork>()), Times.Once);
            courseWorkRepositoryMock.Verify(x => x.SaveChangesAcync(), Times.Once);
        }

        [Test]
        public void GetStudentCourseWorks_ReturnsListOfCourseWorks()
        {
            // Arrange
            var studentId = 1;
            var courseWorks = new List<CourseWork>
    {
        new CourseWork { Id = 1, Theme = "Math", StudentId = studentId, Status = Statuses.New, LecturerId = 2 },
        new CourseWork { Id = 2, Theme = "Science", StudentId = studentId, Status = Statuses.InProgress, LecturerId = 2 }
    };
            var courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();
            courseWorkRepositoryMock.Setup(x => x.GetAllStudentCourseWorks(studentId)).Returns(courseWorks);
            var service = new CourseWorkFacade(courseWorkRepositoryMock.Object, null, null, null);

            // Act
            var result = service.GetStudentCourseWorks(studentId);

            // Assert
            Assert.AreEqual(courseWorks.Count, result.Count);
            Assert.AreEqual(courseWorks[0].Id, result[0].Id);
            Assert.AreEqual(courseWorks[0].Theme, result[0].Theme);
            Assert.AreEqual(courseWorks[0].StudentId, result[0].StudentId);
            Assert.AreEqual(courseWorks[0].Status, result[0].Status);
            Assert.AreEqual(courseWorks[0].LecturerId, result[0].LecturerId);
            Assert.AreEqual(courseWorks[1].Id, result[1].Id);
            Assert.AreEqual(courseWorks[1].Theme, result[1].Theme);
            Assert.AreEqual(courseWorks[1].StudentId, result[1].StudentId);
            Assert.AreEqual(courseWorks[1].Status, result[1].Status);
            Assert.AreEqual(courseWorks[1].LecturerId, result[1].LecturerId);
            courseWorkRepositoryMock.Verify(x => x.GetAllStudentCourseWorks(studentId), Times.Once);
        }

        [Test]
        public void GetStudentCourseWorksByEmail_ShouldReturnCorrectCourseWorks()
        {
            // Arrange
            var expectedCourseWorks = new List<CourseWork>
    {
        new CourseWork { Id = 1, Theme = "Course Work 1", StudentId = 1, Status = Statuses.New, LecturerId = 1 },
        new CourseWork { Id = 2, Theme = "Course Work 2", StudentId = 1, Status = Statuses.InProgress, LecturerId = 2 }
    };
            var student = new Student { Id = 1, Email = "john.doe@example.com", LecturerId = 1 };

            // Mock the dependencies
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name).Returns(student.Email);
            var mockStudentRepository = new Mock<IStudentRepository>();
            mockStudentRepository.Setup(x => x.GetStudentByEmail(student.Email)).Returns(student);
            var mockCourseWorkRepository = new Mock<ICourseWorkRepository>();
            mockCourseWorkRepository.Setup(x => x.GetAllStudentCourseWorksByEmail(student.Email)).Returns(expectedCourseWorks);

            // Create the service instance and call the method
            var service = new CourseWorkFacade(mockCourseWorkRepository.Object, mockHttpContextAccessor.Object, null, mockStudentRepository.Object);
            var result = service.GetStudentCourseWorksByEmail();

            // Assert the result
            Assert.AreEqual(expectedCourseWorks.Count, result.Count);
            for (int i = 0; i < expectedCourseWorks.Count; i++)
            {
                Assert.AreEqual(expectedCourseWorks[i].Id, result[i].Id);
                Assert.AreEqual(expectedCourseWorks[i].Theme, result[i].Theme);
                Assert.AreEqual(expectedCourseWorks[i].StudentId, result[i].StudentId);
                Assert.AreEqual(expectedCourseWorks[i].Status, result[i].Status);
                Assert.AreEqual(expectedCourseWorks[i].LecturerId, result[i].LecturerId);
            }
        }

    }
}
