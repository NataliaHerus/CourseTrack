using AutoMapper;
using BusinessLayer.CourseWorks;
using BusinessLayer.Models;
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
using NSubstitute;
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
        private Mock<IMapper> _mapperMock;

        private ICourseWorkFacade _courseWorkFacade;

        [SetUp]
        public void Setup()
        {
            _studentkRepositoryMock = new Mock<IStudentRepository>();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
            _courseWorkRepositoryMock = new Mock<ICourseWorkRepository>();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity.Name).Returns("test@example.com");
            _httpContextAccessorMock = httpContextAccessorMock;
            _mapperMock = new Mock<IMapper>();

            _courseWorkFacade = new CourseWorkFacade(_courseWorkRepositoryMock.Object, _httpContextAccessorMock.Object,
                _lecturerRepositoryMock.Object, _studentkRepositoryMock.Object, _mapperMock.Object);
        }
        [Test]
        public async Task ChangeCourseWork_UpdatesCourseWorkAndReturnsUpdatedCourseWork()
        {
            var inputDto = new CourseWorkDto { Id = 1, Status = Statuses.InProgress, Theme = "My Coursework" };
            var courseWork = new CourseWork { Id = 1, Status = Statuses.Done, Theme = "Old Coursework" };
            _courseWorkRepositoryMock.Setup(x => x.GetCourseWorkById(inputDto.Id)).Returns(courseWork);

            // Act
            var result = await _courseWorkFacade.ChangeCourseWork(inputDto);

            // Assert
            _courseWorkRepositoryMock.Verify(x => x.UpdateCourseWork(courseWork), Times.Once);
            _courseWorkRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(inputDto.Status, courseWork.Status);
            Assert.AreEqual(inputDto.Theme, courseWork.Theme);
        }


        [Test]
        public async Task CreateCourseWorks_ShouldReturnNewCourseWorkDto()
        {

            var theme = "Test Theme";
            var studentId = 1;

            var lecturer = new Lecturer() { Id = 1, Email = "test@test.com" };
            _lecturerRepositoryMock.Setup(x => x.GetLecturerByEmail(It.IsAny<string>())).Returns(lecturer);

            var newCourseWork = new CourseWork() { Id = 1, Theme = theme, StudentId = studentId, Status = Statuses.New, LecturerId = lecturer.Id };
            _courseWorkRepositoryMock.Setup(x => x.CreateCourseWorks(It.IsAny<CourseWork>())).Callback<CourseWork>(c => newCourseWork = c);

            var expectedCourseWorkDto = new CourseWorkDto()
            {
                Id = newCourseWork.Id,
                Theme = newCourseWork.Theme,
                StudentId = newCourseWork.StudentId,
                Status = newCourseWork.Status
            };


            _mapperMock.Setup(x => x.Map<CourseWorkDto>(It.IsAny<CourseWork>())).Returns(expectedCourseWorkDto);

            // Act
            var result = await _courseWorkFacade.CreateCourseWorks(theme, studentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCourseWorkDto.Id, result.Id);
            Assert.AreEqual(expectedCourseWorkDto.Theme, result.Theme);
            Assert.AreEqual(expectedCourseWorkDto.StudentId, result.StudentId);
            Assert.AreEqual(expectedCourseWorkDto.Status, result.Status);

            _lecturerRepositoryMock.Verify(x => x.GetLecturerByEmail(It.IsAny<string>()), Times.Once);
            _courseWorkRepositoryMock.Verify(x => x.CreateCourseWorks(It.IsAny<CourseWork>()), Times.Once);
            _courseWorkRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<CourseWorkDto>(It.IsAny<CourseWork>()), Times.Once);
        }


        [Test]
        public void GetStudentCourseWorks_ReturnsListOfCourseWorks()
        {
            var studentId = 1;

            var courseWorks = new List<CourseWork>()
{
    new CourseWork() { Id = 1, Theme = "Test Theme 1", StudentId = studentId },
    new CourseWork() { Id = 2, Theme = "Test Theme 2", StudentId = studentId },
    new CourseWork() { Id = 3, Theme = "Test Theme 3", StudentId = studentId }
};

            var expectedCourseWorkDtos = new List<CourseWorkDto>()
{
    new CourseWorkDto() { Id = 1, Theme = "Test Theme 1", StudentId = studentId },
    new CourseWorkDto() { Id = 2, Theme = "Test Theme 2", StudentId = studentId },
    new CourseWorkDto() { Id = 3, Theme = "Test Theme 3", StudentId = studentId }
};

            _courseWorkRepositoryMock.Setup(x => x.GetAllStudentCourseWorks(studentId)).Returns(courseWorks);
            _mapperMock.Setup(x => x.Map<List<CourseWorkDto>>(It.IsAny<List<CourseWork>>())).Returns(expectedCourseWorkDtos);

            // Act
            var result = _courseWorkFacade.GetStudentCourseWorks(studentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCourseWorkDtos.Count, result.Count);

            for (int i = 0; i < expectedCourseWorkDtos.Count; i++)
            {
                Assert.AreEqual(expectedCourseWorkDtos[i].Id, result[i].Id);
                Assert.AreEqual(expectedCourseWorkDtos[i].Theme, result[i].Theme);
                Assert.AreEqual(expectedCourseWorkDtos[i].StudentId, result[i].StudentId);
            }

            _courseWorkRepositoryMock.Verify(x => x.GetAllStudentCourseWorks(studentId), Times.Once);
            _mapperMock.Verify(x => x.Map<List<CourseWorkDto>>(It.IsAny<List<CourseWork>>()), Times.Once);
        }

        [Test]
        public void GetStudentCourseWorksByEmail_ShouldReturnCorrectCourseWorks()
        {
            var student = new Student { Email = "test@example.com" };
            var courseWorks = new List<CourseWork> { new CourseWork { Id = 1, Theme = "Test CourseWork 1" }, new CourseWork { Id = 2, Theme = "Test CourseWork 2" } };
            var courseWorkDtos = new List<CourseWorkDto> { new CourseWorkDto { Id = 1, Theme = "Test CourseWork 1" }, new CourseWorkDto { Id = 2, Theme = "Test CourseWork 2" } };

           _httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity.Name).Returns(student.Email);
            _studentkRepositoryMock.Setup(x => x.GetStudentByEmail(student.Email)).Returns(student);
            _courseWorkRepositoryMock.Setup(x => x.GetAllStudentCourseWorksByEmail(student.Email)).Returns(courseWorks);
            _mapperMock.Setup(x => x.Map<List<CourseWorkDto>>(courseWorks)).Returns(courseWorkDtos);

            // Act
            var result = _courseWorkFacade.GetStudentCourseWorksByEmail();

            // Assert
            Assert.AreEqual(courseWorkDtos, result);
        }
    }
}
