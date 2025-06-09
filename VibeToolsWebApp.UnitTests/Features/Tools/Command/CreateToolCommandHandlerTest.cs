using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Features.Tools.Commands.CreateTool;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Application.MappingProfile;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.UnitTests.Features.Tools.Command
{
    public class CreateToolCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public CreateToolCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ToolsProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateTool_WhenNameIsUnique()
        {
            // Arrange
            var mockRepo = new Mock<IToolRepository>();
            mockRepo.Setup(r => r.SearchAsync("UniqueName"))
                    .ReturnsAsync(new List<Tool>());
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Tool>()))
                    .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<CreateToolCommandHandler>>();
            var handler = new CreateToolCommandHandler(
                mockRepo.Object,
                _mapper,
                mockLogger.Object
            );

            var command = new CreateToolCommand(
                Name: "UniqueName",
                Description: "Test description",
                Category: "TestCategory"
            );

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, resultId);
            mockRepo.Verify(r => r.AddAsync(
                It.Is<Tool>(t =>
                    t.Id == resultId &&
                    t.Name == "UniqueName" &&
                    t.Description == "Test description" &&
                    t.Category == "TestCategory"
                )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenNameAlreadyExists()
        {
            // Arrange
            var existingTool = new Tool
            {
                Id = Guid.NewGuid(),
                Name = "Duplicate",
                Description = "Existing",
                Category = "Cat"
            };
            var mockRepo = new Mock<IToolRepository>();
            mockRepo.Setup(r => r.SearchAsync("Duplicate"))
                    .ReturnsAsync(new List<Tool> { existingTool });

            var mockLogger = new Mock<ILogger<CreateToolCommandHandler>>();
            var handler = new CreateToolCommandHandler(
                mockRepo.Object,
                _mapper,
                mockLogger.Object
            );

            var command = new CreateToolCommand(
                Name: "Duplicate",
                Description: "New description",
                Category: "NewCat"
            );

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(command, CancellationToken.None)
            );
            Assert.Contains("already exists", ex.Message);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Tool>()), Times.Never);
        }
    }
}
