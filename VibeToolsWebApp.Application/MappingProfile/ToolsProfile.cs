using AutoMapper;
using VibeToolsWebApp.Application.Features.Tools.Commands.CreateTool;
using VibeToolsWebApp.Application.Features.Tools.Queries.GetToolDetails;
using VibeToolsWebApp.Application.Features.Tools.Queries.GetTools;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.MappingProfile
{
    public class ToolsProfile : Profile
    {
        public ToolsProfile()
        {
            CreateMap<CreateToolCommand, Tool>();

            // Domain -> ToolDto (for list/query)
            CreateMap<Tool, GetToolsDto>();

            // Domain -> GetToolDetailsDto (for details)
            CreateMap<Tool, GetToolDetailsDto>();
            CreateMap<Review, ReviewDto>();
        }
    }
}
