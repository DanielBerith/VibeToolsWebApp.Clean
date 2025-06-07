using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetTools
{
    public record GetToolsQuery(string? SearchTerm = null) : IRequest<IEnumerable<GetToolsDto>>;

}
