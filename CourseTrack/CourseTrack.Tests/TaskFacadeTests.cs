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
        private Mock<ITaskRepository> _taskRepositoryMock;
        private ITaskFacade _taskService;

        [SetUp]
        public void SetUp()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskFacade(_taskRepositoryMock.Object);
        }

        [Test]
        public void GetTaskById_ValidId_ReturnsTask()
        {
            // Arrange
            var taskId = 1;
            var expectedTask = new Task { Id = taskId, Comment = "Test comment", Status = Statuses.Done, Priority = Priorities.Low };

            _taskRepositoryMock.Setup(x => x.GetTaskById(taskId)).Returns(expectedTask);

            // Act
            var actualTask = _taskService.GetTaskById(taskId);

            // Assert
            Assert.AreEqual(expectedTask, actualTask);
        }

        [Test]
        public void EditTask_ValidTask_CallsEditTaskOnRepository()
        {
            // Arrange
            var taskToEdit = new Task { Id = 1, Comment = "Test comment", Status = Statuses.Done, Priority = Priorities.Low };

            // Act
            _taskService.EditTask(taskToEdit);

            // Assert
            _taskRepositoryMock.Verify(x => x.EditTask(taskToEdit), Times.Once);
        }
    }

}
