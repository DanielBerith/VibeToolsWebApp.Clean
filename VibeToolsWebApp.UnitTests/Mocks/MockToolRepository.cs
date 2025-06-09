using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.UnitTests.Mocks
{
    public static class MockToolRepository
    {
        public static Mock<IToolRepository> GetMock()
        {
            // Seed with some initial tools (reuse MockData or define inline)
            var tools = new List<Tool>
            {
                new Tool
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "ChatGPT",
                    Description = "OpenAI GPT-based conversational AI.",
                    Category = "Model",
                    Reviews = new List<Review>()
                },
                new Tool
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Demo SaaS",
                    Description = "A sample SaaS platform for testing.",
                    Category = "SaaS",
                    Reviews = new List<Review>()
                }
            };

            var mockRepo = new Mock<IToolRepository>();

            // SearchAsync: return all or filtered by Name contains
            mockRepo.Setup(r => r.SearchAsync(It.IsAny<string?>()))
                    .ReturnsAsync((string? term) =>
                    {
                        if (string.IsNullOrWhiteSpace(term))
                            return tools.AsEnumerable();
                        return tools.Where(t =>
                            t.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
                    }
                    );

            // GetByIdAsync: lookup by Id
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) =>
                        tools.FirstOrDefault(t => t.Id == id)
                    );

            // AddAsync: add to list
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Tool>()))
                    .Returns((Tool tool) =>
                    {
                        tools.Add(tool);
                        return Task.CompletedTask;
                    }
                    );

            // UpdateAsync: replace existing
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Tool>()))
                    .Returns((Tool tool) =>
                    {
                        var idx = tools.FindIndex(t => t.Id == tool.Id);
                        if (idx >= 0)
                            tools[idx] = tool;
                        return Task.CompletedTask;
                    }
                    );

            return mockRepo;
        }
    }
}

