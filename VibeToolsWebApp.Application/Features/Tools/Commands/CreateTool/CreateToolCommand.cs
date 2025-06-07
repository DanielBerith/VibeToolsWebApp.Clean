using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Tools.Commands.CreateTool
{
    public record CreateToolCommand(
        string Name,
        string Description,
        string Category
    ) : IRequest<Guid>;
}
