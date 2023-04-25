using AutoMapper;
using BusinessLayer.Models;
using BusinessLayer.Tasks;
using CourseTrack.Controllers;
using DataLayer.Enums;
using DataLayer.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Task = DataLayer.Entities.TaskEntity.Task;

namespace CourseTrack.Tests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _mockTaskRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Task, TaskDto>();
                config.CreateMap<TaskDto, Task>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public void GetStudentTasksById_Should_Return_List_Of_TaskDto()
        {
            // Arrange
            var tasks = new List<Task>()
            {
                new Task() { Id = 1, Comment = "Task 1" },
                new Task() { Id = 2, Comment = "Task 2" },
                new Task() { Id = 3, Comment = "Task 3" }
            };
            _mockTaskRepository.Setup(repo => repo.GetStudentTasks(1)).Returns(tasks);
            var taskService = new TaskFacade(_mapper, _mockTaskRepository.Object);

            // Act
            var result = taskService.GetStudentTasksById(1);

            // Assert
            Assert.IsInstanceOf<List<TaskDto>>(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void GetStudentTasksByEmail_Should_Return_List_Of_TaskDto()
        {
            // Arrange
            var tasks = new List<Task>()
            {
                new Task() { Id = 1, Comment = "Task 1" },
                new Task() { Id = 2, Comment = "Task 2" },
                new Task() { Id = 3, Comment = "Task 3" }
            };
            _mockTaskRepository.Setup(repo => repo.GetStudentTasks("student@example.com")).Returns(tasks);
            var taskService = new TaskFacade(_mapper, _mockTaskRepository.Object);

            // Act
            var result = taskService.GetStudentTasksByEmail("student@example.com");

            // Assert
            Assert.IsInstanceOf<List<TaskDto>>(result);
            Assert.AreEqual(3, result.Count);
        }
    }
}
