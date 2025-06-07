using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Features.Tools.Commands.CreateTool
{
    public class CreateToolCommandHandler : IRequestHandler<CreateToolCommand, Guid>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateToolCommandHandler> _logger;

        public CreateToolCommandHandler(
            IToolRepository toolRepository,
            IMapper mapper,
            ILogger<CreateToolCommandHandler> logger
        )
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateToolCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling CreateToolCommand: Name={Name}, Category={Category}",
                request.Name, request.Category
            );

            // Check for duplicate name
            var existingTools = await _toolRepository.SearchAsync(request.Name);
            if (existingTools.Any(t =>
                    string.Equals(t.Name, request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("Tool creation aborted: a tool with Name='{Name}' already exists.", request.Name);
                throw new InvalidOperationException($"A tool named '{request.Name}' already exists.");
            }

            // Map the incoming command to our domain entity
            var tool = _mapper.Map<Tool>(request);

            // Assign a new Id
            tool.Id = Guid.NewGuid();
            _logger.LogDebug("Assigned new Tool.Id: {ToolId}", tool.Id);

            // Persist the new tool
            await _toolRepository.AddAsync(tool);
            _logger.LogInformation("Persisted new tool with Id={ToolId}", tool.Id);

            // Return the generated Id
            return tool.Id;
        }
    }
}
